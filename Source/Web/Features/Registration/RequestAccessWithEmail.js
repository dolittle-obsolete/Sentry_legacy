/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
import { Command } from '../Command';

export class RequestAccessWithEmail extends Command {
    type='Studio#Sentry.Management-AddIdentityResource+Command|Domain';
    tenant='';
    application='';
    email='';
}