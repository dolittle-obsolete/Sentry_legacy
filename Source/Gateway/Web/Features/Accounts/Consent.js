/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { inject } from 'aurelia-framework';
import { parseQueryString } from 'aurelia-path';
import { ObserverLocator } from 'aurelia-framework';
import { QueryCoordinator } from '@dolittle/queries';
import { CommandCoordinator } from '@dolittle/commands';

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
     * @param {ObserverLocator} observerLocator
     * @param {QueryCoordinator} queryCoordinator
     * @param {CommandCoordinator} commandCoordinator
     */
    constructor(observerLocator, queryCoordinator, commandCoordinator) {
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
    }

    notAllow() {}

    updateGrantedScopes() {
        this.scopes = this.information.identityScopes.filter(scope => scope.checked).map(scope => scope.name.value);
    }

    allow() {}
}