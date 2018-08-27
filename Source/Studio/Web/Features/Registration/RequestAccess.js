/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnect } from "aurelia-open-id-connect";
import { OidcClient, UserManager, WebStorageStateStore } from 'oidc-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import { CommandCoordinator } from '@dolittle/commands';
import { QueryCoordinator } from '@dolittle/queries';
import { RequestAccessWithEmail } from './RequestAccessWithEmail';
import { ProfileClaims } from './ProfileClaims';

const _tenant = new WeakMap();
const _application = new WeakMap();
const _client = new WeakMap();

@inject(Router, CommandCoordinator, QueryCoordinator)
export class RequestAccess {
    isLoggedIn = false;
    name = "";
    profileClaims = [];

    constructor(router, commandCoordinator, queryCoordinator) {
        let self = this;
        this.router = router;
        this._commandCoordinator = commandCoordinator;
        this._queryCoordinator = queryCoordinator;
        let query = new ProfileClaims();
        query.tenant = "be4c4da6-5ede-405f-a947-8aedad564b7f";
        query.application = "CBS"
        queryCoordinator.execute(query).then(result => {
            self.profileClaims = result.items;
        });
    }

    submitRequest() {
        let command = new RequestAccessWithEmail();
        this._commandCoordinator.handle(command).then((result) => {
            var i=0;
            i++;
        });
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

            // http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/CBS/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess

            // Validate params - guids
            // let guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i
            _tenant.set(this, params.tenant);
            _application.set(this, params.application);
            _client.set(this, params.client);

            this._userManager = new UserManager({
                accessTokenExpiringNotificationTime: 1,
                authority: `${window.location.origin}/${params.tenant}/${params.application}`,
                automaticSilentRenew: true,
                checkSessionInternal: 10000,
                client_id: params.client,
                filterProtocolClaims: true,
                loadUserInfo: true,
                post_logout_redirect_uri: '',
                redirect_uri: `${window.location.origin}/Registration/RequestAccessOidcCallback`,
                response_type: 'id_token',
                scope: 'openid email profile',
                silentRequestTimeout: 10000,
                silent_redirect_uri: '',
                userStore: userStore
            });

            this._userManager.getUser().then(user => {
                if( typeof user == 'undefined' || user == null ) return;
                this.isLoggedIn = true;
                this.user = user;

                if( Object.prototype.toString.call(user.profile.name) === '[object Array]' )
                {
                    this.name = user.profile.name[0];
                } else 
                {
                    this.name = user.profile.name;
                }
            });
        }
    }

    login() {
        this._userManager.signinRedirect({
            state: {
                tenant: _tenant.get(this),
                application: _application.get(this),
                client: _client.get(this)
            }
        }).then(request => {
            window.location = request.url;
        });
    }
}
