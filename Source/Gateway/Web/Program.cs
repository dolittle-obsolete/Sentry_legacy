/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Autofac.Extensions.DependencyInjection;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        private static readonly Dictionary<string, string> defaults =
            new Dictionary<string, string> 
            {
                { WebHostDefaults.EnvironmentKey, "Development" }
            };

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            // while (!System.Diagnostics.Debugger.IsAttached) {System.Threading.Thread.Sleep(20);}
            CreateWebHostBuilder(args).Build().Run();
        }

        static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var configuration =
                new ConfigurationBuilder
                ()
                    .AddInMemoryCollection(defaults)
                    .AddEnvironmentVariables("ASPNETCORE_")
                    .AddCommandLine(args)
                    .Build();

            return WebHost.CreateDefaultBuilder(args)
                .UseConfiguration(configuration)
                .UseKestrel()
                .ConfigureServices(services => services.AddAutofac())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>();
        }

        
    }
}
