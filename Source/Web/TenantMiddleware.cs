/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Concepts;
using Microsoft.AspNetCore.Http;
using Read.Management;

namespace Web
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
    public class TenantMiddleware
    {
        readonly Regex _guidRegex = new Regex(@"^(\{{0,1}([0-9a-fA-F]){8}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){4}-([0-9a-fA-F]){12}\}{0,1})$");
        readonly RequestDelegate _next;
        readonly ITenantConfiguration _tenantConfiguration;

        /// <summary>
        /// 
        /// </summary>
        /// /// <param name="_next"></param>
        /// <param name="tenantConfiguration"></param>
        public TenantMiddleware(RequestDelegate _next, ITenantConfiguration tenantConfiguration)
        {
            this._next = _next;
            _tenantConfiguration = tenantConfiguration;
        }

        /// <summary>
        /// The method that gets invoked by the ASP.NET pipeline
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> for the request</param>
        public async Task Invoke(HttpContext context)
        {
            var segments = context.Request.Path.Value.Split('/');
            if (segments.Length > 2)
            {
                TenantId tenantId = null;
                var tenantSegment = segments[1];
                var isGuid = _guidRegex.IsMatch(tenantSegment);
                if (isGuid)
                {
                    tenantId = (TenantId)Guid.Parse(tenantSegment);
                    if (!_tenantConfiguration.HasTenant(tenantId))
                    {
                        throw new ArgumentException("Tenant does not exist");
                        // Todo: redirect to error page with proper error 
                    }

                    var tenant = _tenantConfiguration.GetFor(tenantId);
                    var applicationName = segments[2];
                    if (!tenant.HasApplication(applicationName))
                    {
                        throw new ArgumentException($"Application '{applicationName}' does not exist in tenant '{tenantId.Value}'");
                        // Todo: redirect to error page with proper error 
                    }

                    context.Request.PathBase = new PathString($"/{tenantSegment}/{applicationName}");
                    var remainingSegments = new List<string>(segments);
                    remainingSegments.RemoveRange(0, 3);
                    context.Request.Path = $"/{string.Join('/',remainingSegments)}";
                }
            }
            else
            {

                // Todo: redirect to error page with proper error 

            }
            await _next(context);
        }
    }
}