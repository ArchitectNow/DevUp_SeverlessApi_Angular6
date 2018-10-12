using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ArchitectNow.Identity.IdentityServer;
using ArchitectNow.Core.Mongo;
using ArchitectNow.Mongo.Identity;
using ArchitectNow.Mongo.Identity.Extensions;
using ArchitectNow.Mongo.Identity.Repositories;

namespace ArchitectNow.Identity
{
    public class Startup 
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<MongoConnectionOptions>(Configuration);
            services.Configure<IdentityServerConfiguration>(Configuration.GetSection("IdentityServerConfiguration"));
            
            services.AddCors(opt =>
            {
                opt.AddPolicy("DefaultCors", build => build.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            
            services.AddMongoIdentityRepository<MongoIdentityRepository>();
            services.AddSingleton<IIdentityConfig<AppUser, AppRole>, IdentityConfig>();

            //****** Configure Identity
            var identityBuilder = services.AddIdentity<AppUser, AppRole>(opt =>
                {
                    opt.Password.RequireDigit = true;
                    opt.Password.RequireLowercase = true;
                    opt.Password.RequireNonAlphanumeric = true;
                    opt.Password.RequireUppercase = true;
                    opt.Password.RequiredLength = 8;
                    opt.User.RequireUniqueEmail = true;
                }).AddMongoStores<MongoIdentityRepository>()
                .AddDefaultTokenProviders();

            //****** Configure Identity Server
            var identityServerBuilder = services.AddIdentityServer(opt =>
                {
                    opt.Events.RaiseSuccessEvents = true;
                    opt.Events.RaiseFailureEvents = true;
                    opt.Events.RaiseErrorEvents = true;
                    opt.UserInteraction.LoginUrl = "/auth/login";
                    opt.UserInteraction.LogoutUrl = "/auth/logout";
                })
                .AddProfileService<AmrapsProfileService>()
                .AddClients()
                .AddIdentityApiResources()
                .AddPersistedGrants();
                

            if (Environment.IsDevelopment())
                identityBuilder.SeedUserData();
            
            //Handle certificate for Identity Server
            if (Environment.IsProduction())
                identityServerBuilder.AddSigningCredential(LoadCertificate());
            else
                identityServerBuilder.AddDeveloperSigningCredential();
            //***** End Identity Server Config.
            
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error"); 
            }

            app.UseCors("DefaultCors");
            
            // app.UseAuthentication(); Not needed UseMongoDbForIdentityServer calls this.
            // app.UseIdentityServer(); Not needed UseMongoDbForIdentityServer calls this.
            app.UseMongoDbForIdentityServer();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            
        }
        
        private static X509Certificate2 LoadCertificate()
        {
            //Look up cert in store by thumbprint.
            using (var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                //TODO: Load thumbprint from config or key vault.
                var certCollection = certStore.Certificates.Find(X509FindType.FindByThumbprint, "<THUMBPRINT HERE>", false);
                return certCollection.Count > 0 ? certCollection[0] : null;
            }
        }
    }
}