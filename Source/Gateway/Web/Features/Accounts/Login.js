/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { HttpClient } from 'aurelia-http-client';
import { inject } from 'aurelia-framework';
import { Router } from 'aurelia-router';
import {ExternalAuthoritiesInScheme} from '../Proxies/Authorities/ExternalAuthoritiesInScheme'
import {QueryCoordinator} from '../QueryCoordinator'

/**
 * The view model for the Login view
 */
@inject(QueryCoordinator)
export class Login {
    authorities = []
    tenant = '';

    /**
     * Initializes a new instance of {Login}
     * @param {QueryCoordinator} queryCoordinator 
     */
    constructor(queryCoordinator) {
        this.queryCoordinator = queryCoordinator;
    }

    /**
     * Method that gets called when view and view model is activated.
     */
    activate(params) {
        let self = this;
        this.tenant = params.tenant;
        this.application = params.application;
        
        this.queryCoordinator.execute(new ExternalAuthoritiesInScheme(), params.tenant, params.application)
            .then((result) => {
                let authorities = result.items;

                authorities.forEach(authority => {
                    authority.tenant = self.tenant;
                    authority.application = self.application;
                    authority.returnUrl = `${window.location.origin}/${authority.tenant}/${authority.application}/Accounts/Consent/?tenant=${authority.tenant}&application=${authority.application}&returnUrl=${window.location.origin}/${authority.tenant}/${authority.application}`
                    //40x40 preferred SVG, but png should be accepted
                    
                    if( !authority.logoUrl || authority.logoUrl == '' ) {
                        authority.logoUrl = 'https://azure.microsoft.com/svghandler/information-protection/?width=40&height=40';
                    }
                    self.authorities.push(authority);
                })
            },
            (error) => {
            });
    }
}