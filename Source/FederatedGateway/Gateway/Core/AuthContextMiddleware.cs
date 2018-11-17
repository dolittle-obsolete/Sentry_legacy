/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Concepts;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Read.Infrastructure.Gateway;

namespace Core
{

    /// <summary>
    /// Represents a middleware that is aware of tenancy in the URLs coming in
    /// </summary>
    /// <remarks>
    /// IdentityServer is built to represent a single tenant - or rather, it seems to be very
    /// agnostic towards being tenant aware in the sense that we need it to be.
    /// 
    /// Luckily, under the hood in IdentityServer the PathBase of a request is used actively.
    /// For instance when generating the discovery document, it uses PathBase as prefix.
    /// So for the /.well-known/openid-configuration route it exposes, it uses PathBase to prefix.
    /// 
    /// This middleware supports paths that start with a Guid in the Path and then change the request
    /// path to not have the Guid but then moves the Guid into the PathBase. The Guid represents
    /// the unique identifier of the tenant.
    /// 
    /// In addition to the tenant information, we also want to get the application. This is the next
    /// segment in the path.
    /// 
    /// A valid path would therefor be something like: 
    /// https://dolittle.online/106e84fa-4eed-466b-bb83-70dff8607b4c/someapplication/.well-known/openid-configuration
    /// 
    /// Tenant in this context is the tenant owning the application / client - this is a globally unique identifier
    /// Application is the Dolittle application registration reference for the tenant - this is unique per tenant
    /// </remarks>
    public class AuthContextMiddleware
    {
        /// <summary>
        /// Gets the key used to get the <see cref="AuthContext"/> from the current <see cref="HttpContext"/>
        /// </summary>
        public const string AuthContextItemKey = "AuthContext";
        readonly RequestDelegate _next;
        readonly ITenantConfiguration _tenantConfiguration;
        readonly IHostingEnvironment _hostingEnvironment;
        readonly IAuthenticationHandlerProvider _handlerProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="tenantConfiguration"></param>
        /// <param name="hostingEnvironment"></param>
        /// <param name="handlerProvider"></param>
        public AuthContextMiddleware(
            RequestDelegate next,
            ITenantConfiguration tenantConfiguration,
            IHostingEnvironment hostingEnvironment,
            IAuthenticationHandlerProvider handlerProvider)
        {            
            _next = next;
            _tenantConfiguration = tenantConfiguration;
            _hostingEnvironment = hostingEnvironment;
            _handlerProvider = handlerProvider;
        }

        /// <summary>
        /// The method that gets invoked by the ASP.NET pipeline
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> for the request</param>
        public async Task Invoke(HttpContext context)
        {
            // If Cookie exists: Tenant / Application
            // Else - extract from URL - put in a cookie
            // Rewrite the URL
            
            // Alternative to Cookie: HTTP ETag - session only - https://en.wikipedia.org/wiki/HTTP_ETag
            
            var requestETag = GetEtag(context.Request);

            var segments = context.Request.Path.Value.Split('/').Skip(1).ToArray();

            TenantId tenantId = null;
            Guid tenantGuid = Guid.Empty;
            string tenantSegment = "";
            string applicationName = "";

            if (ETagHasAuthContextInfo(requestETag))
            {
                var eTagSegments = requestETag[0].Split('/');
                tenantSegment = eTagSegments[0];
                applicationName = eTagSegments[1];
            }
            else if (segments.Length > 1)
            {
                tenantSegment = segments[0];
                applicationName = segments[1];
            }
            else throw new ArgumentException("Request path does not have tenantId or applicationName");
            
            var isGuid = Guid.TryParse(tenantSegment, out tenantGuid);
            if (!isGuid) throw new ArgumentException("TenantId could not be parsed to a GUID");
            
            var handler = await _handlerProvider.GetHandlerAsync(context, tenantSegment);
            if( handler != null ) {
                await _next(context);
                return;
            }

            tenantId = tenantGuid;
            if (!_tenantConfiguration.HasTenant(tenantId))
            {
                throw new ArgumentException("Tenant does not exist");
                // Todo: redirect to error page with proper error 
            }

            var tenant = _tenantConfiguration.GetFor(tenantId);
            
            if (!tenant.HasApplication(applicationName))
            {
                throw new ArgumentException($"Application '{applicationName}' does not exist in tenant '{tenantId.Value}'");
                // Todo: redirect to error page with proper error 
            }

            // Set Response ETag
            var responseETag = $"{tenantId.Value}/{applicationName}";
            context.Response.Headers[HeaderNames.ETag] = responseETag;


            context.Request.PathBase = new PathString($"/{tenantSegment}/{applicationName}");
            if( !_hostingEnvironment.IsDevelopment() )
            {
                context.Request.Host = new HostString("dolittle.online");
                context.Request.Scheme = "https";
            }
            
            context.Request.Path = GeneratePath(segments, tenantSegment, applicationName);

            var authContext = new Read.Infrastructure.Gateway.AuthContext(tenant, tenant.Applications[applicationName]);
            //context.Items[AuthContextItemKey] = authContext;
            AuthContextBindings.AuthContext = authContext; 
            
            
            await _next(context);
        }
        StringValues GetEtag(HttpRequest request)
        {
            StringValues values = "";
            request.Headers.TryGetValue("If-None-Match", out values);

            return values;
        }
        bool ETagHasAuthContextInfo(StringValues eTag)
        {
            if (eTag.Count != 1) return false;
            return eTag[0].Split('/').Count() == 2;
        }

        string GeneratePath(string[] requestPathSegments, string tenantId, string applicationName)
        {
            var remainingSegments = new List<string>(requestPathSegments);
            remainingSegments.Remove(tenantId);
            remainingSegments.Remove(applicationName);
            return $"/{string.Join('/',remainingSegments)}";
        }
    }
}