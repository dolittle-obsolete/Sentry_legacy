/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { HttpClient } from 'aurelia-http-client';
import { inject } from 'aurelia-framework';
import { QueryRequest } from '@dolittle/queries/QueryRequest';

@inject(HttpClient)
export class QueryCoordinator {

    constructor(httpClient) {
        this._httpClient = httpClient;

    }

    execute(query, tenantId, applicationName) {
        return this._httpClient.createRequest(`/${tenantId}/${applicationName}/api/Dolittle/Queries`)
            .asPost()
            .withContent(QueryRequest.createFrom(query))
            .send()
            .then(result => {
                let queryResult = JSON.parse(result.response);
                return queryResult;
            });
    }
}