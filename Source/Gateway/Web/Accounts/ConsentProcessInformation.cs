/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;
using Concepts.Scopes;

namespace Web.Accounts
{
    /// <summary>
    /// Represents the information for the consent process
    /// </summary>
    public class ConsentProcessInformation
    {
        readonly List<string> _errors = new List<string>();

        /// <summary>
        /// Gets or sets the name of the client
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// Gets or sets the Url of the client
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the url of the logo for the client
        /// </summary>
        public string ClientLogoUrl { get; set; }

        /// <summary>
        /// Gets or sets whether or not the client allows remembering the consent
        /// </summary>
        public bool AllowRememberConsent { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Scope">scopes</see> related to identity
        /// </summary>
        public IEnumerable<Scope> IdentityScopes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Scope">scopes</see> related to resources
        /// </summary>
        public IEnumerable<Scope> ResourceScopes { get; set; }

        /// <summary>
        /// Gets any errors
        /// </summary>
        public IEnumerable<string> Errors => _errors;

        /// <summary>
        /// Gets whether or not there are errors
        /// </summary>
        public bool HasErrors => Errors.Count() > 1;

        /// <summary>
        /// Add an error
        /// </summary>
        /// <param name="error">Error to add</param>
        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }
}