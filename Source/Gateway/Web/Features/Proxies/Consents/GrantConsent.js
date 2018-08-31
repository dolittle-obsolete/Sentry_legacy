/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated Command Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { Command } from  '@dolittle/commands';

export class GrantConsent extends Command
{
    constructor() {
        super();
        this.type = '24dbdad3-f4c4-429c-b3cf-449cc4287c94';

        this.tenant = '00000000-0000-0000-0000-000000000000';
        this.scopes = [];
        this.returnUrl = '';
        this.rememberConsent = false;
    }
}