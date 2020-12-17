//-------------------------------------------------------------------------
// Copyright © 2020 Province of British Columbia
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-------------------------------------------------------------------------
namespace Keycloak.Authorization.Keycloak.Client.Resource
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Keycloak.Authorization.Keycloak.Client.Configuration;
    using Keycloak.Authorization.Keycloak.Client.Util;
    using Keycloak.Services;

    using Microsoft.Extensions.Logging;

    ///
    /// <summary>Gets the uma2 server configuration settings from the well-known-endpoint.</summary>
    ///
    public class ServerConfigurationResource : IServerConfigurationResource
    {
        private readonly ILogger logger;
        private readonly IHttpClientService httpClientService;

        private readonly IKeycloakConfiguration keycloakConfiguration;

        private ServerConfiguration? serverConfiguration;

        /// <summary>Initializes a new instance of the <see cref="ServerConfigurationResource"/> class.</summary>
        /// <param name="logger">The injected logger.</param>
        /// <param name="httpClientService">The injected httpClientService.</param>
        /// <param name="keycloakConfiguration">The injected keycloak configuration.</param>
        public ServerConfigurationResource(
            ILogger<ServerConfigurationResource> logger,
            IHttpClientService httpClientService,
            IKeycloakConfiguration keycloakConfiguration)
        {
            this.logger = logger;
            this.httpClientService = httpClientService;
            this.keycloakConfiguration = keycloakConfiguration;
        }

        /// <inheritdoc/>
        public ServerConfiguration ServerConfiguration
        {
            get
            {
                if (this.serverConfiguration == null)
                {
                    this.serverConfiguration = this.GetServerConfiguration().Result;
                }

                return this.serverConfiguration;
            }
        }

        /// <summary>Gets the UMA 2.0 Server Configuration from teh well-known Keycloak server end point.</summary>
        /// <returns>An instance of a <see cref="ServerConfiguration"/>.</returns>
        private async Task<ServerConfiguration> GetServerConfiguration()
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            Uri configUri = KeycloakUriBuilder.BuildUri(this.keycloakConfiguration, ServiceUrlConstants.Uma2DiscoveryUrl);

            HttpResponseMessage response = await client.GetAsync(configUri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"getServerConfiguration() returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            ServerConfiguration configurationResponse = JsonSerializer.Deserialize<ServerConfiguration>(result);
            return configurationResponse;
        }
    }
}