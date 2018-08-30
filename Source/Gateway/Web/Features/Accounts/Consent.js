/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnect } from "aurelia-open-id-connect";
import { inject } from 'aurelia-framework';
import { HttpClient } from 'aurelia-http-client';
import { parseQueryString } from 'aurelia-path';
import { ObserverLocator } from 'aurelia-framework';
import {QueryCoordinator} from '../QueryCoordinator'
import {ConsentProcessInformation} from '../Proxies/Consents/ConsentProcessInformation'
import {RetrieveConsentProcessInformation} from '../Proxies/Consents/RetrieveConsentProcessInformation'

/**
 * The view model used for dealing with consent
 */
@inject(OpenIdConnect, ObserverLocator, QueryCoordinator)
export class Consent {
    information={}
    rememberConsent=false;
    scopes=[];
    returnUrl="";
    tenant="";
    application="";

    /**
     * Initializes a new instance of {Consent}
     * @param {OpenIdConnect} openIdConnect 
     */
    constructor(openIdConnect, observerLocator, queryCoordinator) {
        this._openIdConnect = openIdConnect;
        this._observerLocator = observerLocator;
        this._queryCoordinator = queryCoordinator;
    }

    /**
     * Method that gets invoked when view and view model is activated
     */
    activate(routeParams) { 
        let params = parseQueryString(window.location.search.substr(1));
        this.returnUrl = params.returnUrl;
        this.tenant = routeParams.tenant;
        this.application = routeParams.application;

        let setupChecked = (scope) => {
            scope.checked = true;
            this._observerLocator
                .getObserver(scope, 'checked') 
                .subscribe(() => this.updateGrantedScopes());
        };

        let query = new RetrieveConsentProcessInformation();
        query.returnUrl = this.returnUrl;
        this._queryCoordinator.execute(query, this.tenant, this.application)
            .then((result) => {
                this.information = new ConsentProcessInformation(result.items[0]);
                this.information.identityScopes.forEach(setupChecked);
                this.information.resourceScopes.forEach(setupChecked);
                this.updateGrantedScopes();
            },(error) => {
            });
    }

    notAllow() {
    }

    updateGrantedScopes() {
        this.scopes = this.information.identityScopes.filter(scope => scope.checked).map(scope => scope.name.value);
    }

    allow() {
        //TODO: Fire off a GrantConsent/Consent command
        /*
        let self = this;
        let client = new HttpClient();
        client.createRequest("/Consent/Grant")
            .asPost()
            .withContent(scopes)
            .withParams(
                { 
                    returnUrl: this._returnUrl,
                    rememberConsent: this.rememberConsent
                })
            .send();*/
    }
}