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
using Dolittle.Serialization.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Read.Management
{
    /// <summary>
    /// Represents an implementation of <see cref="ITenantConfiguration"/>
    /// </summary>
    public class TenantConfiguration : ITenantConfiguration
    {
        const string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=dolittle;AccountKey=tDIrUNIT24APo6eKQwq0y1WDoT0wq+rqbbxUs+uVHxUi154+/XEgPfpU+DKrDjEm+WPEQ2Z3C3BsQjPLC9a83w==;EndpointSuffix=core.windows.net";
        readonly CloudBlobClient _client;
        readonly CloudBlobContainer _container;
        readonly CloudBlobDirectory _tenantsDirectory;
        readonly ISerializer _serializer;
        readonly ConcurrentDictionary<Guid, Tenant> _tenants = new ConcurrentDictionary<Guid, Tenant>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        public TenantConfiguration(ISerializer serializer)
        {
            _serializer = serializer;

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
                        _tenants[tenant.TenantId] = tenant;
                    }
                })).ToArray();
        
            Task.WaitAll(tasks);
        }
    }
}