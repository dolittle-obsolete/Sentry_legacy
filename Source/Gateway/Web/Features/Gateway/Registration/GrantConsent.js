/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated Command Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { Command } from  '@dolittle/commands';

export class GrantConsent extends Command
{
    constructor() {
        super();
        this.type = 'd526c436-2e50-466d-9db1-19a45c6ba10e';

        this.tenant = '00000000-0000-0000-0000-000000000000';
        this.scopes = [];
        this.returnUrl = '';
        this.rememberConsent = false;
    }
}