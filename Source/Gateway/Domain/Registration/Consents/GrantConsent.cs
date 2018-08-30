using System.Collections.Generic;
using Concepts;
using Concepts.Scopes;
using Dolittle.Commands;

namespace Domain.Registration.Consents
{
    /// <summary>
    /// Represents the intent of the user granting the tenant and application his consent  
    /// </summary>
    public class GrantConsent : ICommand
    {
        /// <summary>
        /// The <see cref="TenantId"/>
        /// </summary>
        /// <value></value>
        public TenantId Tenant {get; set;}
        /// <summary>
        /// The application name
        /// </summary>
        /// <value></value>
        public string Application {get; set;}
        /// <summary>
        /// Gets or sets the name of the client
        /// </summary>
        public string ClientName { get; set; }
        /// <summary>
        /// The <see cref="Scope">Identity Scopes</see> the user consented to
        /// </summary>
        public IEnumerable<Scope> IdentityScopesConsentedTo {get; set;} = new List<Scope>();

        /// <summary>
        /// The <see cref="Scope">Resource Scopes</see> the user consented to
        /// </summary>
        public IEnumerable<Scope> ResourceScopesConsentedTo {get; set;} = new List<Scope>();


    }
}