import { PLATFORM } from 'aurelia-pal';
import style from '../styles/style.scss';
import { OpenIdConnect, OpenIdConnectRoles } from 'aurelia-open-id-connect';
import { inject } from 'aurelia-dependency-injection';

@inject(OpenIdConnect)
export class app {
    constructor(openIdConnect) {
        this._openIdConnect = openIdConnect;
    }

    configureRouter(config, router) {
        config.options.pushState = true;
        config.map([
        ]);

        this._openIdConnect.configure(config);

        this.router = router;
    }
}

//http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/CBS/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess
