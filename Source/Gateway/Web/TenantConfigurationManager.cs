using System.IO;
using Dolittle.Lifecycle;
using Dolittle.Logging;
using Dolittle.Serialization.Json;
using Infrastructure.Tenants;
using Newtonsoft.Json;

namespace Web
{
    /// <inheritdoc/>
    [Singleton]
    public class TenantConfigurationManager : ITenantConfigurationManager
    {
        /// <inheritdoc/>
        public string Path { get; set; }

        readonly ISerializer _serializer;
        readonly ILogger _logger;

        readonly ISerializationOptions _serializationOptions = SerializationOptions.Custom(callback:
            serializer =>
            {
                serializer.ContractResolver = new CamelCaseExceptDictionaryKeyResolver();
                serializer.Formatting = Formatting.Indented;
            }
        );

        /// <summary>
        /// Instantiates an instance of <see cref="TenantConfigurationManager"/>
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="logger"></param>
        public TenantConfigurationManager(ISerializer serializer, ILogger logger)
        {
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public Tenant Load()
        {
            if (!File.Exists(Path)) throw new MissingTenantFile(Path);
            var json = File.ReadAllText(Path);
            return _serializer.FromJson<Tenant>(json, _serializationOptions);
        }
        /// <inheritdoc/>
        public void Save(Tenant tenant)
        {
            if (!File.Exists(Path)) throw new MissingTenantFile(Path);
            var json = _serializer.ToJson(tenant, _serializationOptions);
            File.WriteAllText(Path, json);
        }
    }
}