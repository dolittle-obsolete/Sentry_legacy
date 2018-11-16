/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { OpenIdConnect } from "aurelia-open-id-connect";
import { inject } from 'aurelia-framework';
import { parseQueryString } from 'aurelia-path';
import { ObserverLocator } from 'aurelia-framework';
import {QueryCoordinator} from '../QueryCoordinator'
import {RetrieveConsentProcessInformation} from '../Proxies/Consents/RetrieveConsentProcessInformation'
import { CommandCoordinator } from "../CommandCoordinator";
import { GrantConsent } from "../Proxies/Consents/GrantConsent";

/**
 * The view model used for dealing with consent
 */
@inject(OpenIdConnect, ObserverLocator, QueryCoordinator, CommandCoordinator)
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
    constructor(openIdConnect, observerLocator, queryCoordinator, commandCoordinator) {
        this._openIdConnect = openIdConnect;
        this._observerLocator = observerLocator;
        this._queryCoordinator = queryCoordinator;
        this._commandCoordinator = commandCoordinator;
    }

    /**
     * Method that gets invoked when view and view model is activated
     */
    activate(routeParams) { 
        const params = parseQueryString(window.location.search.substr(1));

        this.returnUrl = params.returnUrl;
        this.tenant = params.tenant;
        this.application = params.application;

        const setupChecked = (scope) => {
            scope.checked = true;
            this._observerLocator
                .getObserver(scope, 'checked') 
                .subscribe(() => this.updateGrantedScopes());
        };

        let query = new RetrieveConsentProcessInformation();
        query.returnUrl = this.returnUrl;
        
        this._queryCoordinator.execute(query, this.tenant, this.application)
            .then((result) => {
                this.information = result.items[0];
                this.information.identityScopes = this.information.identityScopes || new Array();
                this.information.resourceScopes = this.information.resourceScopes || new Array();
                this.information.identityScopes.forEach(setupChecked);
                this.information.resourceScopes.forEach(setupChecked);
                this.updateGrantedScopes();
            },(error) => {
            }
        );
    }

    notAllow() {
    }

    updateGrantedScopes() {
        this.scopes = this.information.identityScopes.filter(scope => scope.checked).map(scope => scope.name.value);
    }

    allow() {
        let command = new GrantConsent()
        command.rememberConsent = this.rememberConsent;
        command.returnUrl = this.returnUrl;
        command.scopes = this.scopes;
        command.tenant = this.tenant;

        this._commandCoordinator.handle(command, command.tenant, this.application)
            .then((commandResult) => {
                if (commandResult.success)
                {
                    //Do something, probably redirect to returnUrl¨
                    console.log(commandResult);
                } else {

                    console.log(commandResult);
                }
            },(error) => {
                console.error("ERROR:", error);
            }
        );
    }
}