/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated ReadModel Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { ReadModel } from  '@dolittle/readModels';

export class ConsentProcessInformation extends ReadModel
{
    constructor() {
        super();
        this.artifact = {
           id: '3402a658-44c1-4ce9-b4c5-918f5eb55948',
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