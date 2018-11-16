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
           id: 'deb36d29-5874-4503-a4af-1c3802febb06',
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