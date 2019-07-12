/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { inject } from 'aurelia-framework';
import { parseQueryString } from 'aurelia-path';
import { ObserverLocator } from 'aurelia-framework';
import { SentryQueryCoordinator } from "../../SentryQueryCoordinator";
import { SentryCommandCoordinator } from "../../SentryCommandCoordinator";
import { RetrieveConsentProcessInformation } from "../Registration/RetrieveConsentProcessInformation";
import { GrantConsent } from "../Registration/GrantConsent";

/**
 * The view model used for dealing with consent
 */
@inject(OpenIdConnect, ObserverLocator, SentryQueryCoordinator, SentryCommandCoordinator)
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
     * @param {ObserverLocator} observerLocator
     * @param {SentryQueryCoordinator} sentryQueryCoordinator
     * @param {SentryCommandCoordinator} sentryCommandCoordinator
     */
    constructor(openIdConnect, observerLocator, sentryQueryCoordinator, sentryCommandCoordinator) {
        this._openIdConnect = openIdConnect;
        this._observerLocator = observerLocator;
        this._queryCoordinator = sentryQueryCoordinator;
        this._commandCoordinator = sentryCommandCoordinator;
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