/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
namespace Web
{
    /// <summary>
    /// Defines the context used throughout
    /// </summary>
    public interface IAuthContext
    {
        /// <summary>
        /// Gets the current <see cref="Tenant"/>
        /// </summary>
        Tenant Tenant { get; }

        /// <summary>
        /// Gets the current <see cref="Application"/>
        /// </summary>
        Application Application { get; }
    }
}