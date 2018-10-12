import {NgModule} from '@angular/core';
import {PageNotFoundComponent} from '../pages/page-not-found.component';
import {AdminLayoutComponent} from './admin.layout.component';
import {NavBarComponent} from '../shared/navbar/nav-bar.component';
import {SideBarComponent} from '../shared/sidebar/side-bar.component';
import {FooterComponent} from '../shared/footer/footer.component';
import {CommonModule} from '@angular/common';
import {BrowserModule} from '@angular/platform-browser';
import {RouterModule} from '@angular/router';
import {MatButtonModule, MatExpansionModule, MatIconModule, MatSidenavModule, MatToolbarModule, MatTooltipModule} from '@angular/material';
import {AuthModule} from '../auth/auth.module';

@NgModule({
  declarations: [
    PageNotFoundComponent,
    AdminLayoutComponent,
    NavBarComponent,
    SideBarComponent,
    FooterComponent
  ],
  imports: [
    AuthModule,
    BrowserModule,
    CommonModule,
    RouterModule,
    MatSidenavModule,
    MatExpansionModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule
  ],
  exports: [
    PageNotFoundComponent,
    AdminLayoutComponent,
    NavBarComponent,
    SideBarComponent,
    FooterComponent
  ]
})
export class LayoutsModule {

}
