/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated ReadModel Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { ReadModel } from  '@dolittle/readmodels';

export class Client extends ReadModel
{
    constructor() {
        super();
        this.artifact = {
           id: '77cacdc0-a6f1-4840-9838-c8a074b8c8ee',
           generation: '1'
        };
        this.id = '00000000-0000-0000-0000-000000000000';
        this.name = '';
        this.allowedGrantTypes = [];
        this.redirectUris = [];
        this.postLogoutRedirectUris = [];
        this.allowedScopes = [];
        this.allowOfflineAccess = false;
        this.requireClientSecret = false;
    }
}