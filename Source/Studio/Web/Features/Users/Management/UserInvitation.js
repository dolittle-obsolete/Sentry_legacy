/*---------------------------------------------------------------------------------------------
 *  This file is an automatically generated ReadModel Proxy
 *  
 *--------------------------------------------------------------------------------------------*/
import { ReadModel } from  '@dolittle/readmodels';

export class UserInvitation extends ReadModel
{
    constructor() {
        super();
        this.artifact = {
           id: 'fe04362e-09c8-45c0-b45e-d05abf9abfd5',
           generation: '1'
        };
        this.id = '00000000-0000-0000-0000-000000000000';
        this.tenantId = '00000000-0000-0000-0000-000000000000';
        this.applicationId = '00000000-0000-0000-0000-000000000000';
        this.email = '';
        this.invited = new Date();
        this.validTo = new Date();
        this.status = 0;
    }
}