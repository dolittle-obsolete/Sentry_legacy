/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using Concepts;
using Dolittle.Tenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Core
{
    public class GatewayHttpRequest
    {
        const string TENANT_HEADER = "d-tenant";
        const string APPLICATION_HEADER = "d-application";
        const int TENANT_SEGMENT = 0;
        const int APPLICATION_SEGMENT = 1;

        public static GatewayHttpRequest ParseFromHttpContext(HttpContext context)
        {
            var tenant = string.Empty;
            var application = string.Empty;
            var fromUrl = false;
            var segments = new string[0];
            if (context.Request.Headers.ContainsKey(TENANT_HEADER)) tenant = context.Request.Headers[TENANT_HEADER];
            if (context.Request.Headers.ContainsKey(APPLICATION_HEADER)) application = context.Request.Headers[APPLICATION_HEADER];

            if( context.Request.Query.ContainsKey("tenant") ) tenant = context.Request.Query["tenant"][0];            
            if( context.Request.Query.ContainsKey("application") ) application = context.Request.Query["application"][0];

            if (string.IsNullOrEmpty(tenant) && string.IsNullOrEmpty(application))
            {
                var etag = context.Request.GetEtag();
                if (etag.Count == 1)
                {
                    var etagContent = etag[0].Split('/');
                    if (etagContent.Length >= 2)
                    {
                        tenant = etagContent[0];
                        application = etagContent[1];
                    }
                }
                else
                {
                    segments = context.Request.Path.Value.Split('/').Skip(1).ToArray();
                    if (segments.Length <= 1) throw new InvalidRequest("No tenant or application name was present in the url or ETag");
                    tenant = segments[TENANT_SEGMENT];
                    application = segments[APPLICATION_SEGMENT];
                }
            }

            Guid tenantId;
            var isGuid = Guid.TryParse(tenant, out tenantId);
            if (!isGuid) throw new InvalidTenantId("TenantId could not be parsed to a GUID");

            return new GatewayHttpRequest(context, tenantId, application, segments, fromUrl);
        }
        public GatewayHttpRequest(HttpContext context, TenantId tenant, ApplicationName application, string[] segments, bool fromUrl)
        {
            Context = context;
            Tenant = tenant;
            Application = application;
            Segments = segments;
            FromUrl = fromUrl;
        }

        public TenantId Tenant { get; }
        public ApplicationName Application { get; }
        public HttpContext Context { get; }
        public string[] Segments { get; }
        public bool FromUrl { get; }

        public void ModifyRequest(bool isHostingEnvironment)
        {
            Context.Request.PathBase = new PathString($"/{Tenant.Value.ToString()}/{Application.Value}");
            if (!isHostingEnvironment)
            {
                Context.Request.Host = new HostString("dolittle.online");
                Context.Request.Scheme = "https";
            }
            if (FromUrl) Context.Request.Path = GeneratePath();
        }

        public void SetEtag()
        {
            var responseETag = $"{Tenant.Value.ToString()}/{Application.Value}";
            Context.Response.Headers[HeaderNames.ETag] = responseETag;
        }

        string GeneratePath()
        {
            var remainingSegments = new List<string>(Segments.Skip(2));
            return $"/{string.Join('/', remainingSegments)}";
        }

    }
}