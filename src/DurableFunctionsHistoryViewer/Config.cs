using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using RazorLightCustom;

namespace DurableFunctionsHistoryViewer
{
    public interface IConnectionStringResolver
    {
        string Resolve(string connectionStringName);
    }
    /// <summary>
    /// Connection string provider which resolves connection strings from the WebJobs context.
    /// </summary>
    internal class WebJobsConnectionStringProvider : IConnectionStringResolver
    {
        private readonly IConfiguration hostConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebJobsConnectionStringProvider"/> class.
        /// </summary>
        /// <param name="hostConfiguration">A <see cref="IConfiguration"/> object provided by the WebJobs host.</param>
        public WebJobsConnectionStringProvider(IConfiguration hostConfiguration)
        {
            this.hostConfiguration = hostConfiguration ?? throw new ArgumentNullException(nameof(hostConfiguration));
        }

        /// <inheritdoc />
        public string Resolve(string connectionStringName)
        {
            return this.hostConfiguration.GetWebJobsConnectionString(connectionStringName);
        }
    }
    [Extension("Dfhv", "Dfhv")]
    public class Dfhv : IExtensionConfigProvider, IAsyncConverter<HttpRequestMessage, HttpResponseMessage>
    {
        private readonly HttpHandler _handler;
        private readonly IRazorLightEngine _razorLightEngine;
        private readonly INameResolver _nameResolver;
        private readonly IConnectionStringResolver _connectionStringResolver;
        private readonly Option _option;
        private readonly CloudTableClient _tableClient;
        private readonly CloudBlobClient _blobClient;

        internal const string DefaultHubName = "DurableFunctionsHub";
        /// <summary>
        /// Gets or sets default task hub name to be used by all <see cref="DurableOrchestrationClient"/>,
        /// <see cref="DurableOrchestrationContext"/>, and <see cref="DurableActivityContext"/> instances.
        /// </summary>
        /// <remarks>
        /// A task hub is a logical grouping of storage resources. Alternate task hub names can be used to isolate
        /// multiple Durable Functions applications from each other, even if they are using the same storage backend.
        /// </remarks>
        /// <value>The name of the default task hub.</value>
        public string HubName { get; set; } = DefaultHubName;
        /// <summary>
        /// Gets or sets the base URL for the HTTP APIs managed by this extension.
        /// </summary>
        /// <remarks>
        /// This property is intended for use only by runtime hosts.
        /// </remarks>
        /// <value>
        /// A URL pointing to the hosted function app that responds to status polling requests.
        /// </value>
        public Uri NotificationUrl { get; set; }

        /// <summary>
        /// Gets or sets the Display DateTime offset hour.
        /// </summary>
        public double OffsetHour => _option.OffsetHour;
        

        public Dfhv(IOptions<Option> options, IRazorLightEngine razorLightEngine, INameResolver nameResolver, IConnectionStringResolver connectionStringResolver)
        {
            _razorLightEngine = razorLightEngine;
            _nameResolver = nameResolver;
            _connectionStringResolver = connectionStringResolver;
            _option = options.Value;
            var connectionName = _option.AzureStorageConnectionStringName ?? ConnectionStringNames.Storage;
            var resolvedStorageConnectionString = _connectionStringResolver.Resolve(connectionName);
            var account = CloudStorageAccount.Parse(resolvedStorageConnectionString);
            _tableClient = account.CreateCloudTableClient();
            _blobClient = account.CreateCloudBlobClient();
            _handler = new HttpHandler(_tableClient, _option.HubName, _razorLightEngine, this);
        }

        void IExtensionConfigProvider.Initialize(ExtensionConfigContext context)
        {
            // For 202 support
            if (this.NotificationUrl == null)
            {
#pragma warning disable CS0618 // Type or member is obsolete
                this.NotificationUrl = context.GetWebhookHandler();
#pragma warning restore CS0618 // Type or member is obsolete
            }            
        }

        Task<HttpResponseMessage> IAsyncConverter<HttpRequestMessage, HttpResponseMessage>.ConvertAsync(HttpRequestMessage input, CancellationToken cancellationToken)
        {
            return _handler.HandleRequestAsync(input);
        }
    }
}
