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
namespace Keycloak.Authorization.Keycloak.Client.Util
{
    using System;
    using System.Globalization;

    using Keycloak.Authorization.Keycloak.Client.Configuration;

    /// <summary>Helper class to build a URL from the well-known URL constant templates for Keycloak.</summary>
    public static class KeycloakUriBuilder
    {
        private const string RealmNameMatch = "{realm-name}";

        /// <summary>Build up the keycloak server url from the template provided and using the configuration settings.</summary>
        /// <param name="configuration">The <see cref="IKeycloakConfiguration"/>.</param>
        /// <param name="template">The template of which '{realm-name}' will be substituted for the configured realm name.</param>
        /// <returns>A new Uri from the the given template url.</returns>
        public static Uri BuildUri(IKeycloakConfiguration configuration, string template)
        {
            string relativePath = template.Replace(RealmNameMatch, configuration.Realm, StringComparison.InvariantCulture);

            return new Uri(configuration.AuthServerUrl!, relativePath);
        }
    }
}