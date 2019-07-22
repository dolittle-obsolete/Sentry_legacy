/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace Core
{
    public static class HttpContextExtensions
    {
        public static void AddTenant(this HttpContext httpContext, AuthenticateResult result)
        {
            var tenantClaim = result.Principal.FindFirst("tid");
            if( tenantClaim != null ) httpContext.Response.Headers.Add("tenant", tenantClaim.Value);
        }


        public static void AddCorrellation(this HttpContext httpContext)
        {
            var correlation = string.Join("",CryptoRandom.CreateRandomKey(32).Select(_ => _.ToString("X")));
            httpContext.Response.Headers.Add("correlation",correlation);
        }

        public static void AddMicroservice(this HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("microservice","68892ab4-f895-0282-efb5-def4ffe422dc");
        }
    }
}