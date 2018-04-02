/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Read.Management;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class MyProfileService : IProfileService
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tenant"></param>
        public MyProfileService(Tenant tenant)
        {
            var i=0;
            i++;

        }

        /// <inheritdoc/>
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            context.AddRequestedClaims(context.IssuedClaims);
            context.AddRequestedClaims(context.Subject.Claims);
            context.AddRequestedClaims(new [] {  new Claim("nationalsociety", "sweden")});

            return Task.CompletedTask;
        }


        /// <inheritdoc/>
        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}