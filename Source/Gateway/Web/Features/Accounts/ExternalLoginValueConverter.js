/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

/**
 * A value converter that takes an authority and generates a correct URL for external login providers
 */
export class ExternalLoginValueConverter {

    /**
     * Convert from an authority to a absolute URL path for logging in with external provider
     * @param {*} authority 
     */
    toView(authority) {
        let url = `${window.location.origin}/${authority.tenant}/${authority.application}/Accounts/ExternalLogin?tenant=${authority.tenant}&application=${authority.application}&authority=${authority.type}&authorityid=${authority.id}&${window.location.search.substr(1)}`;
        return url;
    }
}