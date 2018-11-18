using System;
using System.Collections.Generic;
using System.Linq;
using Concepts;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace Core
{
    public class GatewayHttpRequest
    {
        const int TENANT_SEGMENT = 0;
        const int APPLICATION_SEGMENT = 1;

        public static GatewayHttpRequest ParseFromHttpContext (HttpContext context)
        {
            Guid tenant;
            var etag = context.Request.GetEtag();
            (string[] segments, bool fromEtag) = etag.Count == 1 && etag[0].Split('/').Count() == 2? 
                            (etag[0].Split('/'), true) 
                            : (context.Request.Path.Value.Split('/').Skip(1).ToArray(), false);

            if (segments.Length <= 1) throw new InvalidRequest("No tenant or application name was present in the url or ETag");

            var tenantSegment = segments[TENANT_SEGMENT];
            var applicationSegment = segments[APPLICATION_SEGMENT];
            var isGuid = Guid.TryParse(tenantSegment, out tenant); 
            if (!isGuid) throw new InvalidTenantId("TenantId could not be parsed to a GUID");

            return new GatewayHttpRequest(context, tenant, applicationSegment, segments, fromEtag);

            
        }
        public GatewayHttpRequest(HttpContext context, TenantId tenant, ApplicationName application, string[] segments, bool fromEtag)
        {
            Context = context;
            Tenant = tenant;
            Application = application;
            Segments = segments;
            FromEtag = fromEtag;
        }

        public TenantId Tenant {get; }
        public ApplicationName Application {get; }
        public HttpContext Context {get; }
        public string[] Segments {get; }
        public bool FromEtag {get; }

        public void ModifyRequest(bool isHostingEnvironment)
        {
            Context.Request.PathBase = new PathString($"/{Tenant.Value.ToString()}/{Application.Value}");
            if( !isHostingEnvironment )
            {
                Context.Request.Host = new HostString("dolittle.online");
                Context.Request.Scheme = "https";
            }
            if (!FromEtag) Context.Request.Path = GeneratePath();
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