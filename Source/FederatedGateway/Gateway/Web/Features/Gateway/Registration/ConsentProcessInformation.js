/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated ReadModel Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { ReadModel } from  '@dolittle/readmodels';

export class ConsentProcessInformation extends ReadModel
{
    constructor() {
        super();
        this.artifact = {
           id: '1bfc9724-7f8a-4b98-a86e-7d85f5b33e44',
           generation: '1'
        };
        this.clientName = '';
        this.clientUrl = '';
        this.clientLogoUrl = '';
        this.allowRememberConsent = false;
        this.identityScopes = [];
        this.resourceScopes = [];
    }
}