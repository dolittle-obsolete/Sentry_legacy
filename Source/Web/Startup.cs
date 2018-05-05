/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Autofac;
using Dolittle.Applications.Serialization.Json;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Collections;
using Dolittle.DependencyInversion.Autofac;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Serialization.Json;
using IdentityServer4;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Read.Management;
using Swashbuckle.AspNetCore.Swagger;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomOpenIdConfigurationManager : IConfigurationManager<OpenIdConnectConfiguration>
    {
        static ConcurrentDictionary<string, OpenIdConnectConfiguration> _configurations = new ConcurrentDictionary<string, OpenIdConnectConfiguration>();

        /// <summary>
        /// 
        /// </summary>
        public static string url = "https://login.microsoftonline.com/common/.well-known/openid-configuration";
        //"https://login.microsoftonline.com/tfp/cbsrc.onmicrosoft.com/B2C_1_local/v2.0/.well-known/openid-configuration";


        /// <inheritdoc/>
        public async Task<OpenIdConnectConfiguration> GetConfigurationAsync(CancellationToken cancel)
        {
            if (_configurations.ContainsKey(url)) return _configurations[url];
            var configuration = await OpenIdConnectConfigurationRetriever.GetAsync(url, cancel);
            _configurations[url] = configuration;
            return configuration;
        }

        /// <inheritdoc/>
        public void RequestRefresh()
        {
            _configurations.Keys.ForEach(url =>
            {
                OpenIdConnectConfigurationRetriever.GetAsync(url, CancellationToken.None).ContinueWith(result =>
                {
                    _configurations[url] = result.Result;
                });
            });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class Startup
    {
        readonly IHostingEnvironment _hostingEnvironment;
        IServiceProvider _serviceProvider;

        BootResult _bootResult;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Todo: understand anti forgery
            //services.AddAntiforgery();

            // Todo: RSA Signing Key for Dolittle as Authority

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddCors();

            services.AddMvc();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/accounts/login";
                    options.UserInteraction.LogoutUrl = "/accounts/logout";
                    options.UserInteraction.ConsentUrl = "/accounts/consent";
                    //options.Authentication.CheckSessionCookieName = "sentry.session";
                })

                // Todo: We need our own signing key
                //.AddSigningCredential(credentials)
                .AddDeveloperSigningCredential()

                // Todo: Persist grants per application for the unique user
                .AddInMemoryPersistedGrants()
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddProfileService<ProfileService>();

            services.Add(new ServiceDescriptor(typeof(IConsentMessageStore), typeof(InMemoryConsentMessageStore), ServiceLifetime.Transient));

            services.AddAuthentication()
                // Todo: b2c should have its own provider type, since it has a bunch of different policy things attached to it for signup and signin

                .AddOpenIdConnect("9b296977-7657-4bc8-b5b0-3f0a23c43958", "Azure Active Directory", options =>
                {
                    options.CallbackPath = "/9b296977-7657-4bc8-b5b0-3f0a23c43958/signin-oidc";
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "[Not Set]";
                    options.ClientId = "[Not Set]";

                    // Todo: dynamic scope based on tenant configuration - ask for the same as configured with
                    options.Scope.Clear();
                    options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                    options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                    options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);

                    options.ConfigurationManager = new CustomOpenIdConfigurationManager();

                    options.Events.OnRedirectToIdentityProvider = async(context) =>
                    {
                        var query = context.HttpContext.Request.Query;
                        var authorityId = Guid.Parse(query["authorityid"]);
                        var tenantId = Guid.Parse(query["tenant"]);
                        var applicationName = query["application"];

                        var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                        var tenant = tenantConfiguration.GetFor(tenantId);
                        var application = tenant.Applications[applicationName];

                        var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                        context.Options.TokenValidationParameters.ValidAudience = authority.ClientId;                  

                        // Todo: Dynamically create a new instance of OpenIdConnectMessage for the actual configuration - this could potentially fall over with multiple users accessing the system at once
                        context.ProtocolMessage.ClientId = authority.ClientId;
                        // Todo: rename autority.Secret to client secret
                        //context.ProtocolMessage.ClientSecret = authority.Secret;

                        // Todo: Guide on setting up an app for Azure - include this https://apps.dev.microsoft.com/

                        if (!_hostingEnvironment.IsDevelopment()) context.ProtocolMessage.RedirectUri = $"https://dolittle.online{options.CallbackPath}";
                        await Task.CompletedTask;
                    };

                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Todo: We should validate everything!!
                        ValidateIssuer = false,

                        // Todo: set the correct claim types depending on the target - Azure AD has a different claim for name than others
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            _bootResult = services.AddDolittle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddDolittle(_bootResult.Assemblies, _bootResult.Bindings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _serviceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Todo: this probably is a bit too lose.. 
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            app.UseMiddleware<AuthContextMiddleware>();

            app.Use(async(context, next) =>
            {
                var query = context.Request.Query;
                if (query.ContainsKey("tenant") && query.ContainsKey("application") && query.ContainsKey("authority"))
                {
                    var authorityId = Guid.Parse(query["authorityid"]);
                    var tenantId = Guid.Parse(query["tenant"]);
                    var applicationName = query["application"];

                    var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                    var tenant = tenantConfiguration.GetFor(tenantId);
                    var application = tenant.Applications[applicationName];
                    var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                    var url = $"{authority.Authority}/.well-known/openid-configuration";
                    CustomOpenIdConfigurationManager.url = url;
                }

                await next();
            });
            app.UseIdentityServer();

            app.UseMvc();
            app.UseDolittle();

            app.RunAsSinglePageApplication();
        }
    }
}

// be4c4da6-5ede-405f-a947-8aedad564b7f - Tenant       - Red Cross
// 25c7ddac-dd1b-482a-8638-aaa909fd1f1c - Application  - CBS
// 
// Authorities:
// 9b296977-7657-4bc8-b5b0-3f0a23c43958 - Azure Active Directory

// https://login.microsoftonline.com/cbsrc.onmicrosoft.com/v2.0

// https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oidc
// https://login.microsoftonline.com/{tenant}/v2.0/.well-known/openid-configuration
// https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0/.well-known/openid-configuration

// http://localhost:5000/Accounts/ExternalLogin?tenant=be4c4da6-5ede-405f-a947-8aedad564b7f&authority=undefined&returnUrl=%2Fbe4c4da6-5ede-405f-a947-8aedad564b7f%2FCBS%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3D25c7ddac-dd1b-482a-8638-aaa909fd1f1c%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A5000%252FRegistration%252FRequestAccessOidcCallback%26response_type%3Did_token%26scope%3Dopenid%2520email%2520profile%26state%3Dfcecbea8cd2b47f98ef62b9b5f2d0827%26nonce%3D1c0a46ff32c94b19939801eb80e8152f
// https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid&response_mode=form_post&nonce=636591209858467760.YzQzYWYyN2EtNDYzNC00MTVjLTk1MWEtZThmYTEwM2UyYWRjZjRmYmNkMmUtMjVkMS00M2IwLWE3ZDYtODk2N2EwYTMyOTYz&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYKdpNv3E-TyMKiDmG3FTgquI3eRUXiNpAWCG0Qgtui5MS7IJnLABEID0pBQotfmPwQ_m39pPYLtFsttS4qhSm8tsf5uSCNSlxjiwEqLk6QQKAT7FZrKOLTEMf7PyRnVMS6pNJTMMuBMZCJRXqVdmWzZnz__ckRAvcgm_I5ALOskbVZEZjIU9jXbMJlxcw82m89aIVADv2YoH7g3IVWd0FexAXbo9kQAGhtbz_xyP7ojFQ-WEVIyDvJBe3I_ptOtou9bO9Jyow7D4nFfG_75B2SubcyE9Zb-t_8YqWTo9BEPBHr9ayGxG21GHnHkgeJmQDpaSwU5BXl7bjUXYvtH_rPm1mCWd0EBgFLU02ENE9U-FqnRnyvXj3essbTKVMxm79rc8x1ZMGoop-9HnsAhS34JZIuPUB_6wHPo33QsDAPbUsMHZljhgWyp7li0P-1UOORJnKWtmNm6W0C3Fc7543L7K6sMLx42lMhbzFLp7dv2B1PF-zz6LqRFZ9fZF3NE1aNECEe8U_HHgJrWYf2-BrVEXtG17160tlSFM-QGX4xvif7qXKIpmxrd3Lnr8mxAESyXp2szlsF8MwGwvHb7k3U5QSc02MVTRoNlJt_47wKXTzbu2NGS8lrRWO3yztTDEZXcAFuylnBiwlNYJRttyqFkeQI3UCEgheyYtLMBQnABracfB3NjpwmtPRlnNuMYLYf9b9_49XSJ3xiFr884oLh0ZDfkOVdn3rLUI85O8uMugUZsn61rL7BERIRmwrMXgYdPaaEPantCsTOArt2bSvOKGVKJbDauvybvcLJnlxpDGFgVl71X99DhBV9MVfGjgC3AipO27ZOvyOUVaFZdRDDoEKHqe7wouGI4_zPS_RLyZ_MVhL9Nb8jMwFnF1DfsNteJXJL6Fx8-CTB2H_sAOOP5&x-client-SKU=ID_NET&x-client-ver=2.1.4.0

// https://login.microsoftonline.com/cbsrc.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_local&client_id=0734a066-658a-47e6-aea5-a036c997b61f&nonce=defaultNonce&redirect_uri=https%3A%2F%2Fdolittle.online%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&scope=openid&response_type=id_token&prompt=login
// https://login.microsoftonline.com/te/cbsrc.onmicrosoft.com/b2c_1_local/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid%20email%20profile&response_mode=form_post&nonce=636591440521285590.ZjE0ZGZjZGMtOGFjYy00N2RhLWFiZjItMWVlODRlYjY2YTA1M2M4ZDI1ZDItMjUwNC00MTJjLTk3MTktMzczNTE4N2EwNzkz&client_secret=%3B%7Dj%7C%5BX5%7C6NP%7D%60_Ek3J1x%3Dow2&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYJR7SI-ruLVfwVqbJDahWqNYYDmdl1NVNsWntp9cKjzIM6QEKr1oDlAa6bspp4DMiAqjXe6cl8o3m21Yr0XCw7OMDM0AyXAYqVqmuJrBP0MVOtxgKUCVQ6CLP3ZJ3BKMVdcckzGmsKl-QEfif1_wZSM8EI9HTle2Mz8Ee1yVSSeH5OXiWvGeeSjPViBtH19wyqGgD1dZ8qxLHqa6TdpiEVpTGuZaXremboDj6ubZKmo0bm1LshxhZn24j71aZaqtv5Xl9F9LH-C3q-Urunt4OXkYwHzn3Fk-wVo__qICAVKql863dgzLcHYX8m1FKWCUSosLeOKY-kG1Gqyk4VXUhzPj2cMNmSTTWsg727iyhT1Unjn_U4rGK8SFpuct8xYONkG9GtKyYvPie1MOoVihjVgW17Sj1SIWFK8UsFq9a15NRamIc0uChJEGgI2WS1iIUixvm7_20t-QBXuzUX4s_aLOSn-LwzXwDrWQWjMMa1BzwhugAyHhWbAbSAfhqUbS951pkIZWFwKHNfgp_4YRbzLVdZV68uQkRR7dAfF3v1YRIFfxfCuoGcIVO5gZfXO0V1Qgr6QjCOPtVqeEBvBXnjlHKyaekWAzZ6MhaIXbGjbyzlZ15g3EollpWdSWqXZ2lQ4-7yV6uQ3tgiexnZcBIQHeDR2IvG3vVRZJCt15jNM_bo56aOz3oE-VCLvJxqXHwf1Tf6BwK-V3rVZYmHgZrh5Vbrd82-N_lbwd7fv-0VL-l7Roo_R7lBumrFrcyel55Ty6BOMjlH4ut0NZ_-XyxMjgdq7SpZ6rmSwnc3c4-pQcDdaRqw_NCHCxdj2FD8rErmUqaHgDYg3bgjOVYWSwURd-rwXx7WEFwkn2wLgiS33hoswIZBjjYKvfbMweJ-T7JO-danFFg7Gz7P7-DV0CgGm&x-client-SKU=ID_NET&x-client-ver=2.1.4.0
// https://login.microsoftonline.com/te/cbsrc.onmicrosoft.com/b2c_1_local/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=https%3A%2F%2Fdolittle.online%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid%20email%20profile&prompt=login&nonce=defaultNonce
// https://login.microsoftonline.com/te/cbsrc.onmicrosoft.com/b2c_1_local/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=https%3A%2F%2Fdolittle.online%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid%20email%20profile&response_mode=form_post&nonce=defaultNonce                    

// https://login.microsoftonline.com/dbed8645-9f1f-4ccc-9e61-4e7f92e2a27d/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&scope=openid%20email&response_type=id_token
// https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fsignin-oidc-9b296977-7657-4bc8-b5b0-3f0a23c43958&response_type=id_token&scope=openid&response_mode=form_post&nonce=636584610907046420.YTBmNjNmZDYtZDA5Yy00ODc0LThmYzYtNzA1MTM5MDdkY2I1NWE5NDI4YzQtZjQ4NC00MTQ0LTk1NWYtZmM0YTNkZTRkNjcz&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYJ0JbP1AHhNgbBRK8uDeBXs8aAAbmg4xcJqOO3Ee66CuCzyS88FDJjAI7C6ik2wa_lBP4F0uOwoB9NkFZF92suDklX2PZw9WNXgGKhHtK0HrE15Z2Y96y7S8F25isk0CP8--EDkIT2viijS8hlMl0nR_xb8IJBZYbn30GWHp_iRYkxmmFABuJtPHX79YA6U7p8HTbJQvHZkXCoUyu_L9d4GjLDoXItDBRSHZ0ZFVopyF8DJLMacuA6ox85DuAzKnZ8odF8GasMhvIlRqQiyQ5kBsczn120VOh2JtCELP-a7w5TIP-pf6LpWk4zdGEbem1taVvnsMTEESRPBheX1Yl8vKnHh1BOYFe0eWNspVemV3D3x23OCI3ON_oVmD9oG7x2xpJ5oP71h9dKCI4NoyL1dPQ3bKw06xQn6Qol-fZxXszbw2GJhCKGwz26Rf_HtSR8Ur4hKjbVoSBzqBlHDmOkuQagGuD8PJbUzd2fgANrxFxIe4tWc9EPtEuPigbNRfGY_E5vIOrG3rv2OyLVhDCNoJi8FVOIM9czYYMYXHdRkjq0vQzwwwjQIjOep_dMjeR3NcxShGHDcXuikoQ_JapHSyMw-VAmhWFMNYCMkka4HpgiD35PYSzpDJ39NvS1G8TUAgPJahhLRotdwG26WVKP3sMY_hmyrfMH4-lXCo7r7F02BHrWNNwr4xuA5JxQlBLE8xJARecqno6zqPWCLOoqafDs450HqhfHekP3AjvSSzlvHAG89XEWBl8s8ViBj3mGh0uMK6Re1fHX-kEX8q0pv5a4UOSR38UdAT6Cju4RRML5Xj6x19vfRw69sfNRtYQkTmtgvBvjbBoazQ9Z2R1rWGKvsVSbbWrew3lFS-cvqEocQlLALWxAy78AJW0V1EmJbwUf3k9M0uPzpbv4T87WkKMBpa73FQU1yyQJ6-GZReQ&x-client-SKU=ID_NET&x-client-ver=2.1.4.0
// https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fsignin-oidc-9b296977-7657-4bc8-b5b0-3f0a23c43958&response_type=id_token&scope=openid&response_mode=form_post
// https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid&response_mode=form_post&nonce=636591211675680420.YjE1NDRjN2ItZTFjMC00NmI1LWI5OGQtYzM0MjY0MmIyODE4ODJkMTc1ZWQtYWY4NC00ODFjLTllZjItOTE2MTZiMmVmZjEw&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYJQJ0C-jpMoYJHavARcSHieaMNQzneaFQ4jlVKhIWpgcQzTwy0wy9VFX8C1QzjFIu57mu0CXk7QslObNna4_8awqm-cuvBNVPvB_zJC_UDHWYRXl6SS7byWrZTVRPCb3XPtIyPNmo1Y95-5CZ7tN9MkV_-3YhZUo3vw08c8KTDVy1mD4WOTdqGb7la2fuX8__AlxsmUGBK8kLZjoXbZtwFvqISoehSmUuCcl4V9tIasZZjADijQBzcN-wk5aBXMHU6RCPpRPa-7zzNmJhNP7fEPERSx4czrB1oUJK1UQ7QmGH1FwOESx5unnKvYlAHIBuxo2rO9P8qmzDYS6QLEMP7o-KvkAwzB6BveEOYukpgrkYc2Z-aly2ZFcSs1Cucu0HwrSTe-kqUv7cZBaeuz1t4tT5MxpUxE_X9KR1r_x1XqqIiat1AiRVJzZbwSI5eOYtYiupnWO23NUqVfGunNucrHiAObwVEkL4l2_P2BjOuoX5YuRwYjxmi7lAS0gFlwRNfgakBTUYlPh1_f3lpqCN-_V8nTzt6oTzrI1pDgZVcQQHfQSOc_gT6qgxgF5t3AR0PFr67VSw54Imihjq53Zfv58kk8igpyoFXRMXJhoONmGu7VBX9zc6PXPza4tCar1jvmgfKAjZQVfkoHfKqs0cQwDi3YLMYdWdRsnuz5-rGgCkt2UcK2DTWqgJzwlim6mOWwOsKvAJ2PXME2Gt7o8DUiPgsPG6C83o-F1BvH9VUbpPd6VxqX6YLzBFXeVmRNXua1pdm809GhfLHHyI6x_gPEub8yQCl8MFZBIqTzlSDdoR6cfMTe6_pyENdPgFwUl8nYEEH-JUBsJm8O4eZmTwAscPNsCTnwpdLjDyG6C7e1pI6IGfEjbuhXemb3vMvjeKJ03KMFCY4QTtOG3b1pfNED&x-client-SKU=ID_NET&x-client-ver=2.1.4.0
// https://login.microsoftonline.com/cbsrc.onmicrosoft.com/oauth2/v2.0/authorize?p=B2C_1_local&client_id=0734a066-658a-47e6-aea5-a036c997b61f&nonce=defaultNonce&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&scope=openid&response_type=id_token&prompt=login

// https://login.microsoftonline.com/te/cbsrc.onmicrosoft.com/b2c_1_local/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2F9b296977-7657-4bc8-b5b0-3f0a23c43958%2Fsignin-oidc&response_type=id_token&scope=openid&response_mode=form_post&nonce=636591224042648990.YWRlNzgzZjEtOWNlZC00MDQxLTlmYWYtN2Y4OWQ2Yjg5NTFlN2EzNGE3OGMtZGI5Ny00MzA4LTg0MzAtZGJiMGQwMmVlM2Fi&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYIpR_XxkCjpfJ2NfF-ua0qd2auhJoGfBncyNLdVC1LQ2mUbWn-LpkQiS6fk3F80TyuK7EE9KuoFf0edWoiL4jMmQr-nuDYEzJd4IJ2ZJm03PeoFzJhHA6ZFGOkKQ_lRbVkWF5pC5oAZdog162GWNpAPoDB1eFAW65l6Me-W3ZQINwpZxM80LO0XjUf9JMPxiNxm1OFhz7mC2AMf5ShpXevLVPPH64clvWgovfAEuGSzqSqfN_x33GfK8wxEmuXm7BPoE9iTv9uIXGpR-rvo18PMMgl3eA7p6Il3zJVErVQeCBUQIEjfwKyf7eJ_p0YYRo23KDswJ6JENdqijkuGOsRrE3hV_iF5hRoT657qUL8qmTmvdh9ipHE9kmnwy8VRVZMT2OQUTjTyIHgBQHbkuQJ6KKLmR33g87vz_TNnL485fnz0fpwXN3nOyZ1Xojm2jmN6QOsq2Vu5P7SaWqEoP-ihTFy74dkdRzBTjyk6jrmbozGd16VnY_XjJ97-mg8_Np_xrWgfkUkkFSkqmHzZk-BNjosop7fRbXKHL92ZOz4UJmQKeR6cq7KfkIvFj5rrGM3yVQRHSUjIj9jafcj38IysGsfsVkkkaMjy6GQv9yqpgpKePFTlYnp8b4-N-PXHKsxXnDvDNdNtLyJUR5fuNOObasDlm1Ya6wbbF_VLZJkEWqbOA5IkpdftKPv2Tf-lTWAGPrXU9bwE4QgjXwcEBXRrG4Yn-inxsnB4Pi_hzqt6am3W9IcSrRsM-wsd42Bsexir6Ek5LUz05HKAtrmyIIigjMs7qajUM595iTQ2ohSwpsHS2pIFa40EMUEkin3czDtUJwS0EF6SgcXI76FMmCYMoWPogvNv5Q8JgaGt3L_0b1tKpnFEtuKtd6o8IQnYAQr0n2oM4c8Qh_tIl3Tt4Cwg&x-client-SKU=ID_NET&x-client-ver=2.1.4.0

// http://dolittle.online/be4c4da6-5ede-405f-a947-8aedad564b7f/CBS/connect/authorize?client_id=25c7ddac-dd1b-482a-8638-aaa909fd1f1c&redirect_uri=http%3A%2F%2Fdev.cbsrc.org%2F&response_type=id_token&scope=openid%20email%20profile&state=332dc2698d75497584230d1422840571&nonce=8399d458dc914660b06e6a0ba98ff62e 

/*

,
                {
                    "Id": "f4366163-ade0-485f-85fa-3065341f23f4",
                    "TenantId": "381088c1-de08-4d18-9e60-bbe2c94eccb5",
                    "ClientId": "e4f46937-110a-4693-9a5b-1e521af81192",
                    "Authority": "https://login.microsoftonline.com/common/v2.0",
                    "Type": "9b296977-7657-4bc8-b5b0-3f0a23c43958",
                    "DisplayName": "Multitenant Azure AD",
                    "Secret": "",
                    "LogoUrl": ""
                },                
                {
                    "Id": "f52b452b-105d-4ccd-b95b-9fe0e7733439",
                    "TenantId": "19901569-9bba-4fe3-8a0b-1fab6c80c151",
                    "ClientId": "cbs-staging",
                    "Authority": "https://auth.humanitarian.id",
                    "Type": "9b296977-7657-4bc8-b5b0-3f0a23c43958",
                    "DisplayName": "Humanitarian Id",
                    "Secret": "",
                    "LogoUrl": ""
                },
                {
                    "Id": "92727dde-685d-4780-81ae-4981f2bc7798",
                    "TenantId": "381088c1-de08-4d18-9e60-bbe2c94eccb5",
                    "ClientId": "2e2cad73-c11a-4d9f-8af9-beeebcdc5a27",
                    "Authority": "https://login.microsoftonline.com/common/v2.0",
                    "Type": "9b296977-7657-4bc8-b5b0-3f0a23c43958",
                    "DisplayName": "Dolittle",
                    "Secret": "",
                    "LogoUrl": ""
                }
 */