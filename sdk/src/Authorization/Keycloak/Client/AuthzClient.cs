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
namespace Keycloak.Client
{
    using Keycloak.Client.Configuration;
    using Keycloak.Client.Resource;
    using Keycloak.Services;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// This is class serves as an entry point for clients looking for access to Keycloak Authorization Services.
    /// When creating a new instances make sure you have a Keycloak Server running at the location specified in the client
    /// configuration. The client tries to obtain server configuration by invoking the UMA Discovery Endpoint, usually available
    /// from the server at http(s)://{server}:{port}/auth/realms/{realm}/.well-known/uma-configuration.
    /// In the org.keycloak Java libs, this class is called 'AuthZClient'.
    /// </summary>
    public class AuthorizationClient
    {
        /// <summary>
        /// The injected logger delegate.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// The injected HttpClientService delegate.
        /// </summary>
        private readonly IHttpClientService httpClientService;

        /// <summary>
        /// The keycloak configuration.
        /// </summary>
        private readonly IKeycloakConfiguration keycloakConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationClient"/> class.
        /// </summary>
        /// <param name="logger">The injected logger provider.</param>
        /// <param name="httpClientService">injected HTTP client service.</param>
        /// <param name="keycloakConfiguration">Injected keycloak Configuration.</param>
        /// <param name="serverConfigurationResource">Injected server configuration resource.</param>
        /// <param name="authorizationResource">Injected authorization resource.</param>
        /// <param name="protectionResource">Injected protection resource.</param>
        public AuthorizationClient(
            ILogger<AuthorizationClient> logger,
            IHttpClientService httpClientService,
            IKeycloakConfiguration keycloakConfiguration,
            IServerConfigurationResource serverConfigurationResource,
            IAuthorizationResource authorizationResource,
            IProtectionResource protectionResource)
        {
            this.logger = logger;
            this.httpClientService = httpClientService!;
            this.keycloakConfiguration = keycloakConfiguration;

            // publicly exposed but injected resources.
            this.ServerConfiguration = serverConfigurationResource;
            this.Protection = protectionResource;
            this.Authorization = authorizationResource;
        }

        /// <summary>Gets the UMA 2.0 server configuration resource.</summary>
        public IServerConfigurationResource ServerConfiguration { get; }

        /// <summary>Gets the protection resource.</summary>
        public IProtectionResource Protection { get; }

        /// <summary>Gets the authorization resource.</summary>
        public IAuthorizationResource Authorization { get; }
    }
}