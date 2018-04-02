/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnect } from "aurelia-open-id-connect";
import { OidcClient, UserManager, WebStorageStateStore } from 'oidc-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

const _tenant = new WeakMap();
const _application = new WeakMap();

@inject(Router)
export class RequestAccess {
    isLoggedIn = false;

    constructor(router) {
        this.router = router;
    }
    
    activate(params, route, navigationInstruction) {
        let userStore = new WebStorageStateStore({
            prefix: "requestaccess",
            store: window.localStorage
        });

        if (navigationInstruction.fragment == '/Registration/RequestAccessOidcCallback') {
            let userManager = new UserManager({
                userStore: userStore
            });
            userManager.signinRedirectCallback().then(user => {
                userManager.storeUser(user);
                this.router.navigateToRoute('RequestAccess', user.state);
            });
        } else {

            // Validate params - guids
            // let guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i
            _tenant.set(this, params.tenant);
            _application.set(this, params.application);

            this._userManager = new UserManager({
                accessTokenExpiringNotificationTime: 1,
                authority: `http://localhost:5000/${params.tenant}`,
                automaticSilentRenew: true,
                checkSessionInternal: 10000,
                client_id: params.application,
                filterProtocolClaims: true,
                loadUserInfo: true,
                post_logout_redirect_uri: '',
                redirect_uri: `http://localhost:5000/Registration/RequestAccessOidcCallback`,
                response_type: 'id_token',
                scope: 'openid profile',
                silentRequestTimeout: 10000,
                silent_redirect_uri: '',
                userStore: userStore
            });

            this._userManager.getUser().then(user => {
                if( typeof user == "undefined" || user == null ) return;
                this.isLoggedIn = true;
            });
        }
    }

    login() {
        this._userManager.signinRedirect({
            state: {
                tenant: _tenant.get(this),
                application: _application.get(this)
            }
        }).then(request => {
            window.location = request.url;
        });
    }
}