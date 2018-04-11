import { PLATFORM } from 'aurelia-pal';
import style from '../styles/style.scss';
import { OpenIdConnect, OpenIdConnectRoles } from "aurelia-open-id-connect";
import { inject } from 'aurelia-dependency-injection';

@inject(OpenIdConnect)
export class app {
    constructor(openIdConnect) {
        this._openIdConnect = openIdConnect;
    }

    configureRouter(config, router) {
        config.options.pushState = true;
        config.map([
            { route: ['', 'welcome'], name: 'welcome', moduleId: PLATFORM.moduleName('welcome') },
            { route: ':tenant/:application/Accounts/Login', name: 'Login', moduleId: PLATFORM.moduleName('Accounts/Login') },
            { route: ':tenant/:application/Accounts/Consent', name: 'Consent', moduleId: PLATFORM.moduleName('Accounts/Consent') },
            { route: ['Registration/RequestAccessOidcCallback', ':tenant/:application/:client/Registration/RequestAccess'], name: 'RequestAccess', moduleId: PLATFORM.moduleName('Registration/RequestAccess') },
            { route: ':tenant/:application/Registration/Requests', name: 'Requests', moduleId: PLATFORM.moduleName('Registration/Requests'), settings: { cssClass: "wide-column" } }
        ]);

        this._openIdConnect.configure(config);

        this.router = router;
    }
}

//http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/CBS/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess