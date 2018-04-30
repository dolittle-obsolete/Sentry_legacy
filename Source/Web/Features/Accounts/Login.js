/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { HttpClient } from 'aurelia-http-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';

/**
 * The view model for the Login view
 */
@inject(Router)
export class Login {
    authorities = []
    tenant = '';

    /**
     * Initializes a new instance of {Login}
     * @param {Router} router 
     */
    constructor(router) {
        this.router = router;
    }


    /**
     * Method that gets called when view and view model is activated.
     */
    activate(params) {
        let self = this;
        this.tenant = params.tenant;
        this.application = params.application;

        let client = new HttpClient();
        client.get(`/${params.tenant}/${params.application}/Authorities`)
            .then(data => {
                let authorities = JSON.parse(data.response);

                authorities.forEach(authority => {
                    
                    // Todo: serialization is not hooked up
                    authority.type = authority.type;
                    authority.tenant = self.tenant;
                    authority.application = self.application;
                    //40x40 preferred SVG, but png should be accepted
                    
                    if( !authority.logoUrl || authority.logoUrl == '' ) {
                        authority.logoUrl = 'https://azure.microsoft.com/svghandler/information-protection/?width=40&height=40';
                    } 
                    self.authorities.push(authority);
                });
            });
    }
}