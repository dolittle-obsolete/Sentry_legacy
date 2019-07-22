/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

/**
 * The view model used for selecting tenant to log onto
 */
export class TenantSelector {
    select(tenant) {
        if (document.location.search.indexOf('userCode') >= 0) {
            document.location = `/device/signin?tenant=${tenant}`;
        } else {
            document.location = `/auth/signin?tenant=${tenant}`;
        }
    }
}