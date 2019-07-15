/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { PLATFORM } from 'aurelia-pal';
import style from '../styles/style.scss';
import { OpenIdConnect, OpenIdConnectRoles } from 'aurelia-open-id-connect';
import { inject } from 'aurelia-dependency-injection';
import { QueryCoordinator } from '@dolittle/queries';
import { Router } from 'aurelia-router';

class TenantAndApplicationStep {
    static tenant = '';
    static application = '';

    run(routingContext, next) {
        TenantAndApplicationStep.tenant = routingContext.params.tenant;
        TenantAndApplicationStep.application = routingContext.params.application;
        return next();
    }
}

@inject(OpenIdConnect, Router)
export class app {
    #openIdConnect;
    #globalRouter;
    

    constructor(openIdConnect, router) {
        this.#openIdConnect = openIdConnect;
        this.#globalRouter = router;
    }

    configureRouter(config, router) {
        config.options.pushState = true;
        config.map([
            { route: ['', ':tenant/:application', ':tenant/:application/welcome'], name: 'welcome', moduleId: PLATFORM.moduleName('welcome'), layoutView: PLATFORM.moduleName('layout.html') },
            { route: ':tenant/:application/Accounts/Login', name: 'Login', moduleId: PLATFORM.moduleName('Accounts/Login') },
            { route: ':tenant/:application/Accounts/Consent', name: 'Consent', moduleId: PLATFORM.moduleName('Accounts/Consent') },
            { route: ':tenant/:application/Device/Verify', name: 'Verify', moduleId: PLATFORM.moduleName('Devices/Verify') }
        ]);
        config.addPreActivateStep(TenantAndApplicationStep);

        this.#openIdConnect.configure(config);

        QueryCoordinator.beforeExecute(options => {
            options.headers['d-tenant'] = TenantAndApplicationStep.tenant;
            options.headers['d-application'] = TenantAndApplicationStep.application;
        });

        this.router = router;
    }
}

//http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/CBS/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess
