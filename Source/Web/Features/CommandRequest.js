/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Command } from './Command';
import { Guid } from './Guid';

/**
 * Represents a request for issuing a {Command}
 */
export class CommandRequest {
    correlationId = '';
    type = '';
    content = {};

    /**
     * Initializes a new instance of {CommandRequest}
     * @param {string} type 
     * @param {*} content 
     */
    constructor(type, content) {
        this.correlationId = Guid.create();
        this.type = type;
        this.content = content;
    }

    /**
     * Creates a {CommandRequest} from a {Command}
     * @param {Command} command 
     */
    static createFrom(command) {
        var request = new CommandRequest(command.type, command);
        return request;
    }
}