/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

import { inject } from 'aurelia-framework';
import { CommandCoordinator } from '@dolittle/commands/CommandCoordinator';
import { Command } from '@dolittle/commands/Command';


@inject(CommandCoordinator)
export class SentryCommandCoordinator
{
    /**
     * Creates an instance of SentryCommandCoordinator.
     * @param {CommandCoordinator} commandCoordinator
     * @memberof SentryCommandCoordinator
     */
    constructor(commandCoordinator) {
        
        this._commandCoordinator = commandCoordinator;
    }
    /**
     * Handles a command in the context of sentry
     *
     * @param {Command} command
     * @param {string} tenantId
     * @param {string} applicationName
     * @returns {any} The command result
     * @memberof SentryCommandCoordinator
     */
    handle(command, tenantId, applicationName) {
        console.log('handling command');
        console.log(command);
        console.log(tenantId);
        console.log(applicationName);
        CommandCoordinator.apiBaseUrl = `/${tenantId}/${applicationName}`;
        
        return this._commandCoordinator.handle(command)
            .then( commandResult => commandResult)
            .catch( error => error);
    }
}