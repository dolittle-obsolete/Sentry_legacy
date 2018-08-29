/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Concepts;
using Dolittle.Collections;
using Dolittle.Execution;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Infrastructure;
using Infrastructure.Tenants;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Web
{
    //TODO: This is obsolete when we decide to mount the configuration files and using them directly, then most of the Infrastructure code will go as well.
    /// <summary>
    /// Represents an implementation of <see cref="ITenantConfiguration"/>
    /// </summary>
    [Singleton]
    public class TenantConfiguration : ITenantConfiguration
    {
        const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=dolittle;AccountKey=tDIrUNIT24APo6eKQwq0y1WDoT0wq+rqbbxUs+uVHxUi154+/XEgPfpU+DKrDjEm+WPEQ2Z3C3BsQjPLC9a83w==;EndpointSuffix=core.windows.net";
        readonly CloudBlobClient _client;
        readonly CloudBlobContainer _container;
        readonly CloudBlobDirectory _tenantsDirectory;
        readonly ISerializer _serializer;
        readonly ConcurrentDictionary<Guid, Tenant> _tenants = new ConcurrentDictionary<Guid, Tenant>();
        readonly ILogger _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
        public TenantConfiguration(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;

            var storageAccount = CloudStorageAccount.Parse(ConnectionString);
            _client = storageAccount.CreateCloudBlobClient();
            _container = _client.GetContainerReference("sentry");
            _container.CreateIfNotExistsAsync().Wait();
            _tenantsDirectory = _container.GetDirectoryReference("tenants");
            DownloadTenantsConfiguration().Wait();
        }

        /// <inheritdoc/>
        public bool HasTenant(TenantId tenantId)
        {
            return _tenants.ContainsKey(tenantId);
        }

        /// <inheritdoc/>
        public Tenant GetFor(TenantId tenantId)
        {
            return _tenants[tenantId];
        }

        /// <inheritdoc/>
        public void Save(Tenant tenant)
        {
            _tenants[tenant.TenantId] = tenant;

            var blobName = GetTenantBlobNameFor(tenant.TenantId);
            var blob = _tenantsDirectory.GetBlockBlobReference(blobName);
            var jsonAsString = _serializer.ToJson(tenant);
            blob.Properties.ContentType = "application/json";
            blob.UploadTextAsync(jsonAsString);
        }

        string GetTenantBlobNameFor(TenantId tenantId)
        {
            return $"{tenantId.Value}.json";
        }

        async Task DownloadTenantsConfiguration()
        {
            BlobContinuationToken continuationToken = null;
            var tenants = new List<IListBlobItem>();
            do
            {
                var response = await _tenantsDirectory.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                tenants.AddRange(response.Results);
            } while (continuationToken != null);

            var tasks = tenants.Select(listBlob => 
                Task.Run(async()=>
                {
                    var tenantBlob = listBlob as CloudBlockBlob;
                    if (tenantBlob != null)
                    {
                        var jsonAsString = await tenantBlob.DownloadTextAsync();
                        var tenant = _serializer.FromJson<Tenant>(jsonAsString);
                        _logger.Information($"Loaded tenant '{tenant.Name}' with id '{tenant.TenantId}'");

                        _tenants[tenant.TenantId] = tenant;
                        tenant.Applications.ForEach(application => 
                        {
                            _logger.Information($"With application '{application.Key}'");
                            application.Value.Clients.ForEach(client =>
                            {
                                _logger.Information($"With client '{client.Name}' with id '{client.Id}'");

                            });


                        });
                    }
                })).ToArray();
        
            Task.WaitAll(tasks);
        }
    }
}