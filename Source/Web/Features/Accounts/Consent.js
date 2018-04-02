/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnect } from "aurelia-open-id-connect";
import { inject } from 'aurelia-framework';
import { HttpClient } from 'aurelia-http-client';
import { parseQueryString } from 'aurelia-path';
import { ObserverLocator } from 'aurelia-framework';

/**
 * The view model used for dealing with consent
 */
@inject(OpenIdConnect, ObserverLocator)
export class Consent {
    information={};
    rememberConsent=false;
    scopes=[];
    returnUrl="";
    tenant="";
    application="";

    /**
     * Initializes a new instance of {Consent}
     * @param {OpenIdConnect} openIdConnect 
     */
    constructor(openIdConnect, observerLocator) {
        this._openIdConnect = openIdConnect;
        this._observerLocator = observerLocator;
    }

    /**
     * Method that gets invoked when view and view model is activated
     */
    activate(routeParams) { 
        let self = this;
        let client = new HttpClient();
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
        
        client.createRequest(`/${routeParams.tenant}/${routeParams.application}/Consent`)
            .asGet()
            .withParams({returnUrl: params.returnUrl})
            .send()
            .then(data => {
                self.information = JSON.parse(data.response);
                self.information.identityScopes.forEach(setupChecked);
                self.information.resourceScopes.forEach(setupChecked);
                self.updateGrantedScopes();
            });
    }

    notAllow() {
    }

    updateGrantedScopes() {
        this.scopes = this.information.identityScopes.filter(scope => scope.checked).map(scope => scope.name.value);
    }

    allow() {
        

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