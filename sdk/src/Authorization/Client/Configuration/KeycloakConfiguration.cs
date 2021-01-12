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
namespace Keycloak.Client.Configuration
{
    using System;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Class that represents the OpenId Configuration model for the Keycloak Configuration.
    /// </summary>
    public class KeycloakConfiguration : IKeycloakConfiguration
    {
        private const string ConfigurationSectionKey = "Keycloak";
        private readonly IConfiguration configuration;

        /// <summary>Initializes a new instance of the <see cref="KeycloakConfiguration"/> class.</summary>
        /// <param name="configuration">The injected <see cref="IConfiguration"/> configuration object.</param>
        public KeycloakConfiguration(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.configuration.Bind(ConfigurationSectionKey, this);
        }

        /// <inheritdoc/>
        public string Audience { get; set; } = string.Empty;

        /// <inheritdoc/>
        public Uri? AuthServerUrl { get; set; }

        /// <inheritdoc/>
        public string Realm { get; set; } = string.Empty;
    }
}