/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

// http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess
// http://localhost:5000/Accounts/ExternalLogin?tenant=be4c4da6-5ede-405f-a947-8aedad564b7f&authority=9b296977-7657-4bc8-b5b0-3f0a23c43958&returnUrl=/be4c4da6-5ede-405f-a947-8aedad564b7f/25c7ddac-dd1b-482a-8638-aaa909fd1f1c/Registration/RequestAccess
// http://localhost:5000/be4c4da6-5ede-405f-a947-8aedad564b7f/accounts/login?returnUrl=%2Fbe4c4da6-5ede-405f-a947-8aedad564b7f%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dmvc%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A5002%252Fsignin-oidc%26response_type%3Did_token%26scope%3Dopenid%2520profile%2520nationalsociety%26response_mode%3Dform_post%26nonce%3D636580270518155460.MjI2OTU2MTYtNGEyOS00ZGMxLTg3M2QtNGY3YzkxMmNkYWQ3OWYzNDQzYWMtOTQwNC00YjUxLTk3MjQtYjkwNzZkOGQ5YzE0%26state%3DCfDJ8CeXhV5JBLFMkMDAcLC1jYI8Xd8UMzP8_lNkrTtPHM62ETRtnakGcB_Qh8tyh_IsjdOwl53w7uYX8HvIHoy44G6WOwR-8DOKzV7jZDYUuQJ2suL-9rcjcFIGVeaUbrzLEoXQczRjn530Pxy4yeT-SjZQKP_tD1_MbWcm6zXKUsshGmhS8FZhseHuVQDDgxH18flWDQBUYMbiduaUOySUY-hoT_2UYlRH2ytLE2_diYqAO05OV267nnEWVo3UydEhfMuGWRguKc8qYy0Fhc74TGIf4PgmIaRZrRTSQBxyI8zMc081WXY8XZO0sSAPADuYmTkeFzNQ3QD90_ALapXKIjU%26x-client-SKU%3DID_NET%26x-client-ver%3D2.1.4.0


// https://zamboni-app.azurewebsites.net/signin-oidc#state=12b6c198aea7409fb8447522ed65a62d&scope=openid%20email%20profile%20roles&token_type=Bearer&access_token=CfDJ8OANmcN2MqxIrNychIQQJle1r0pukfqUzKIimCA30x4TJEU7SooBzKeQW6qM39F00lSbafTLHFmZP93T4GkNpf0mwcojTWEsuYrp3xu3uF5wS1g50C4uS3U8aaYsx141pGMhcNoTbcMZxF0qUOS962iBagKUNEj50k3gxQGG-NRIzmxisDAbt2LkgVt9w_IwG9vhiFEGthnYzXDMxBBHlVsNHBvaXlmQADk7aLANn4Y9M7rtW6TScK6MFJqInEmafMTXG-Dmid3AmrwTelEPkvkxArk78UFe7MIbaWyOAU31BTyMfnqFh4RiDUqmQKtjLvqw3OPZnju0qJ-gX-2FnFqN89DMU8IMCXhogQcsLxJJYfrONind1eLhc06O6OuUe4vCAT8fJZET9ywOTCoylS_SeAXYf-oFP3F2JEMBtvjv_35A7NJN62NBPK6qD6O3qUw4N8GEsqtOjTGxxyJOO85xbEIXErELip7bOxqfqUHvRlsjjzrwn-yLil03XuBNYhRu0EjznAqKE7ouM8qJku3OxsVGH9qjwAOZCyUsdDVv5IZc6yH0z5us6Bs9HTUfDKtwDOgECFYOI1JERKGLDF8lzLcr0SMZOs4VWkkA0HB2ryeah0DBUzXRFhE19UZMRYZMu49atuQZnTlbPZSfX-6jqR3yRAgAE5kTCWbtqNITjop-gVy2cJsOdFZpnLiDDvka_3z-aOFoyya9SOMF8Yo566jbLTri1suSFmOKL1JgN70OWx3YF1tE9TlT3II486I6H4PZsD6V33YqhIiCRTnnhk-mq5NfNE7l6Fmc1ttPzXMg9fxMzvDMlhNYXLK1JxUW9_1ct_XKGJ-G011EkgN1oQOcp7c8DElJI3UVn-AWODoPrLG0hyYq3vDuZpyBAUel8J8VMnKMOwbHMr_SnSycItP9B3ybHGYuxr7vpDxFkbR3fQXEgyY1NY0NqA9rIZSz2VVDB_kbiL-OSkff5Wodi2fGNOd7oD-LijdXC2c4EXDNZcgo3oKpQRJYzNxUw0rF8u4nk5tbh3hQZGByrS9aQYP6-sspJYZqNPsLfWSL7SgoPbra0gXlLL7l08-Mt3g7SRxa7AxZy7e9zzhMmsWc6eBWnDRdRY9_Ovo1x2I3GDIvGu92uy4tcVvxxk-IDGrw4T4iS2aogHmGIEVgvsA&expires_in=3600&id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IlVKU01BQVpYTUpVVk1LT0FVSVZIUERTS1lQVTNEVkpDS0ZLRTNCMVIiLCJ0eXAiOiJKV1QifQ.eyJ1bmlxdWVfbmFtZSI6ImVpbmFyQGRvbGl0dGxlLmNvbSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiOTRlYTlhODQtYWJkYy00NzJjLTg3OWYtMDc0Y2VhYzgwNTdkIiwic3ViIjoiMzZjOTY1YjItNWVhNi00NzRkLTgwYmMtMzVlMzE3NjZiOTA0IiwianRpIjoiYTA5ODJlMzAtNmMxYy00ZmM5LWJkNzYtMmI5NTM5ZjUyMzdlIiwidXNhZ2UiOiJpZGVudGl0eV90b2tlbiIsImF1ZCI6IkF1cmVsaWEuT3BlbklkQ29ubmVjdCIsIm5vbmNlIjoiYWM5YjE5YWFhMGVkNGY5ODhjOTdiNDI5YmE1NGYxYmMiLCJhdF9oYXNoIjoiM1VtNjkzM2h6RGlFU3BiQndpQjNLdyIsImF6cCI6IkF1cmVsaWEuT3BlbklkQ29ubmVjdCIsIm5iZiI6MTUyMjQzMTA5MywiZXhwIjoxNTIyNDMyMjkzLCJpYXQiOjE1MjI0MzEwOTMsImlzcyI6Imh0dHBzOi8vemFtYm9uaS1hdXRoLmF6dXJld2Vic2l0ZXMubmV0LyJ9.cAysgTxvvRS7r2UIN_1fnVEV_b16vmXthpW5KZr5MdZngR5uJBAgymmtygXuHvVeXkWpRx7mjGUUIb3WUlFhp4NeknkH2f0ntoqx0qzC-3ce_POYHtjfsBsnmKSNswF3Y32yfiYDlZGXs_mBrJ1OBkMueMl51vvsk8Op6QXi4dLOwNEg6ssBCA0WxTWsXXUK-AkXoX8WnwMCPYbmDFCu0FbmRFzXoPCi9pztIyux5TpemujUmBOhdGIrIsKocBca7kgWafzaoqOq5cGFMiKTIFdOYPB7hEn7tRcT0Ia7HPbLJolBOcD0lU3N8mAvhRPHNaQCii0hsDF_Duv3cmUxcg

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
    loginRedirectRoute: 'welcome',
    logoutRedirectRoute: 'welcome',
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