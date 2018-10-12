export class CoreConfig {
    authenticationAuthority: string;
    apiBaseUrl: string;
    baseUrl: string;

    constructor() {
        this.authenticationAuthority = 'http://localhost:4000';
        this.baseUrl = 'http://localhost:4001';
        this.apiBaseUrl = 'http://localhost:4001/api';
    }
}
