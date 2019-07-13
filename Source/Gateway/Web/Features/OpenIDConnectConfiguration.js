/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnectConfiguration } from 'aurelia-open-id-connect';
import { UserManagerSettings, WebStorageStateStore } from "oidc-client";

const appHost = window.location.origin;

let tenant = '';
let application = '';
let guidRegex = /^[0-9a-f]{8}-[0-9a-f]{4}-[1-5][0-9a-f]{3}-[89ab][0-9a-f]{3}-[0-9a-f]{12}$/i
//new Regex("^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
let segments = window.location.pathname.split('/');
if( segments.length > 1 )
{
    if( segments[1].match(guidRegex) ) {
        tenant = segments[1];
    }
}

if( segments.length > 1 && segments[1].match(guidRegex) ) tenant = segments[1];
if( segments.length > 2 && segments[2].match(guidRegex) ) application = segments[2];

tenant = 'be4c4da6-5ede-405f-a947-8aedad564b7f';
application = '25c7ddac-dd1b-482a-8638-aaa909fd1f1c';

export default {
    unauthorizedRedirectRoute: `/${tenant}/Accounts/Login`,
    //Accounts/Login?returnUrl=/login`,
    ///be4c4da6-5ede-405f-a947-8aedad564b7f/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess`,
    loginRedirectRoute: '/welcome',
    logoutRedirectRoute: '/welcome',
    userManagerSettings: {
        accessTokenExpiringNotificationTime: 1,
        
        authority: `${window.location.origin}/${tenant}`,
        automaticSilentRenew: true,
        checkSessionInterval: 10000,
        client_id: application,
        filterProtocolClaims: true,
        loadUserInfo: false,
        post_logout_redirect_uri: `${appHost}/signout-oidc`,
        redirect_uri: `${appHost}/signin-oidc`,
        response_type: "id_token",
        scope: "openid profile",
        silentRequestTimeout: 10000,
        silent_redirect_uri: `${appHost}/signin-oidc`,
        userStore: new WebStorageStateStore({
            prefix: "oidc",
            store: window.localStorage,
        })
    }
};