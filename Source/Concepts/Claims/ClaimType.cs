/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/

namespace Concepts.Claims
{
    /// <summary>
    /// Rerepsents the type of resource
    /// </summary>
    public enum ClaimType
    {
        /// <summary>
        /// Value is of string type
        /// </summary>
        String,

        /// <summary>
        /// Value is of boolean type
        /// </summary>
        Boolean,
        
        /// <summary>
        /// Value is of a select type
        /// </summary>
        Select
    }
}