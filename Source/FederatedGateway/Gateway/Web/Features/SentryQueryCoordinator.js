/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

import { inject } from 'aurelia-framework';
import { QueryCoordinator } from '@dolittle/queries/QueryCoordinator';
import { Query } from '@dolittle/queries/Query';

@inject(QueryCoordinator)
export class SentryQueryCoordinator {
    /**
     * Creates an instance of SentryQueryCoordinator.
     * @param {QueryCoordinator} queryCoordinator
     * @memberof SentryQueryCoordinator
     */
    constructor(queryCoordinator) {
        this._queryCoordinator = queryCoordinator;

    }
    /**
     * Executes a query in the sentry context
     *
     * @param {Query} query
     * @param {string} tenantId
     * @param {string} applicationName
     * @returns {any} Query result
     * @memberof SentryQueryCoordinator
     */
    execute(query, tenantId, applicationName) {
        QueryCoordinator.apiBaseUrl = `/${tenantId}/${applicationName}`;
        console.log('executing query');
        console.log(query);
        console.log(tenantId);
        console.log(applicationName);
        return this._queryCoordinator.execute(query)
            .then( queryResult => queryResult)
            .catch( error => error);
    }
}