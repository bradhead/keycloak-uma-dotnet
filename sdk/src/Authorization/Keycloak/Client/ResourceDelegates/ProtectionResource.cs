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
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Keycloak.Authorization.Keycloak.Client.Configuration;
    using Keycloak.Authorization.Keycloak.Client.Representation;
    using Keycloak.Authorization.Keycloak.Client.Util;
    using Keycloak.Services;

    using Microsoft.Extensions.Logging;

    ///
    /// <summary>An entry point for managing permission tickets using the Protection API.</summary>
    ///
    public class ProtectionResource : IProtectionResource
    {
        private readonly ILogger logger;

        /// <summary>The injected HttpClientService.</summary>
        private readonly IHttpClientService httpClientService;

        /// <summary>The keycloak configuration settings read from the injected app settings.</summary>
        private readonly IKeycloakConfiguration keycloakConfiguration;

        /// <summary>The keycloak UMA server configuration.</summary>
        private readonly IServerConfigurationResource serverConfigurationDelegate;

        /// <summary>Initializes a new instance of the <see cref="ProtectionResource"/> class.</summary>
        /// <param name="logger">The injected logger provider.</param>
        /// <param name="httpClientService">injected HTTP client service.</param>
        /// <param name="keycloakConfiguration">The keycloak configuration.</param>
        /// <param name="serverConfigurationDelegate">The injected server-side configuration settings.</param>
        public ProtectionResource(
            ILogger<ProtectionResource> logger,
            IKeycloakConfiguration keycloakConfiguration,
            IServerConfigurationResource serverConfigurationDelegate,
            IHttpClientService httpClientService)
        {
            this.logger = logger;
            this.keycloakConfiguration = keycloakConfiguration;
            this.serverConfigurationDelegate = serverConfigurationDelegate;
            this.httpClientService = httpClientService;
        }

        /// <inheritdoc/>
        public async Task<TokenIntrospectionResponse> IntrospectRequestingPartyToken(string rpt, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.IntrospectionEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("requesting_party_token", "token_type_hint");
            dict.Add("token", rpt);
            using (HttpContent content = new FormUrlEncodedContent(dict))
            {
                content.Headers.Add(@"Content-Type", @"application/x-www-form-urlencoded");

                HttpResponseMessage response = await client.PostAsync(new Uri(requestUrl), content).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogError($"introspectRequestingPartyToken() returned with StatusCode := {response.StatusCode}.");
                }

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                TokenIntrospectionResponse introspectionResponse = JsonSerializer.Deserialize<TokenIntrospectionResponse>(result);
                return introspectionResponse;
            }
        }
    }
}