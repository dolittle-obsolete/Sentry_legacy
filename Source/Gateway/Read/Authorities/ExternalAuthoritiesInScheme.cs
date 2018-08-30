using System.Linq;
using Dolittle.Queries;
using Infrastructure;
using Infrastructure.Authorities;
using Microsoft.AspNetCore.Authentication;

namespace Read.Authorities
{
    /// <summary>
    /// Represents an <see cref="IQuery"/> that queries the <see cref="ExternalAuthority"/>s that matches the scheme
    /// </summary>
    public class ExternalAuthoritiesInScheme : IQueryFor<ExternalAuthority>
    {
        readonly IAuthenticationSchemeProvider _schemeProvider;
        readonly IAuthContext _authContext;

        /// <summary>
        /// Instantiates an instance of <see cref="ExternalAuthoritiesInScheme"/>
        /// </summary>
        public ExternalAuthoritiesInScheme(IAuthenticationSchemeProvider schemeProvider, IAuthContext authContext)
        {
            _schemeProvider = schemeProvider;
            _authContext = authContext;
        }

        /// <summary>
        /// The query
        /// </summary>
        public IQueryable<ExternalAuthority> Query
        {
            get
            {
                var schemes = _schemeProvider.GetAllSchemesAsync().Result;
                 return _authContext
                        .Application
                            .ExternalAuthorities
                            .Where(
                                authority => 
                                    schemes.Any(scheme => scheme.Name == authority.Type.ToString()
                                )
                            ).AsQueryable();
            }
        }

    }
}