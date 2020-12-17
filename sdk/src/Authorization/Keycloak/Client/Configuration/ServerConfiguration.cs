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
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    /// <summary>
    /// Class that represents the user model in the UMA 2 Configuration as fetched from the well-known endpoint.
    /// </summary>
    public class ServerConfiguration
    {
        /// <summary>
        /// Gets or sets the user issuer.
        /// </summary>
        [JsonPropertyName("issuer")]
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the authorization_endpoint.
        /// </summary>
        [JsonPropertyName("authorization_endpoint")]
        public string AuthorizationEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the token_endpoint.
        /// </summary>
        [JsonPropertyName("token_endpoint")]
        public string TokenEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the introspection_endpoint.
        /// </summary>
        [JsonPropertyName("introspection_endpoint")]
        public string IntrospectionEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the userinfo_endpoint.
        /// </summary>
        [JsonPropertyName("userinfo_endpoint")]
        public string UserinfoEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the end_session_endpoint.
        /// </summary>
        [JsonPropertyName("end_session_endpoint")]
        public string LogoutEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the jwks_uri.
        /// </summary>
        [JsonPropertyName("jwks_uri")]
        public Uri? JwksUri { get; set; }

        /// <summary>
        /// Gets or sets the check_session_iframe.
        /// </summary>
        [JsonPropertyName("check_session_iframe")]
        public string CheckSessionIframe { get; set; } = string.Empty;

        /// <summary>
        /// Gets the grant_types_supported.
        /// </summary>
        [JsonPropertyName("grant_types_supported")]
        public List<string> GrantTypesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the response_types_supported.
        /// </summary>
        [JsonPropertyName("response_types_supported")]
        public List<string> ResponseTypesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the subject_types_supported.
        /// </summary>
        [JsonPropertyName("subject_types_supported")]
        public List<string> SubjectTypesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the id_token_signing_alg_values_supported.
        /// </summary>
        [JsonPropertyName("id_token_signing_alg_values_supported")]
        public List<string> IdTokenSigningAlgValuesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the userinfo_signing_alg_values_supported.
        /// </summary>
        [JsonPropertyName("userinfo_signing_alg_values_supported")]
        public List<string> UserInfoSigningAlgValuesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the request_object_signing_alg_values_supported.
        /// </summary>
        [JsonPropertyName("request_object_signing_alg_values_supported")]
        public List<string> RequestObjectSigningAlgValuesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the response_modes_supported.
        /// </summary>
        [JsonPropertyName("response_modes_supported")]
        public List<string> ResponseModesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the registration_endpoint.
        /// </summary>
        [JsonPropertyName("registration_endpoint")]
        public string RegistrationEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets the token_endpoint_auth_methods_supported.
        /// </summary>
        [JsonPropertyName("token_endpoint_auth_methods_supported")]
        public List<string> TokenEndpointAuthMethodsSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the token_endpoint_auth_signing_alg_values_supported.
        /// </summary>
        [JsonPropertyName("token_endpoint_auth_signing_alg_values_supported")]
        public List<string> TokenEndpointAuthSigningAlgValuesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the claims_supported.
        /// </summary>
        [JsonPropertyName("claims_supported")]
        public List<string> ClaimsSupported { get; } = new List<string>();

        /// <summary>
        /// Gets the claim_types_supported.
        /// </summary>
        [JsonPropertyName("claim_types_supported")]
        public List<string> ClaimTypesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the  claims parameter is supported.
        /// </summary>
        [JsonPropertyName("claims_parameter_supported")]
        public bool ClaimsParameterSupported { get; set; }

        /// <summary>
        /// Gets the scopes_supported.
        /// </summary>
        [JsonPropertyName("scopes_supported")]
        public List<string> ScopesSupported { get; } = new List<string>();

        /// <summary>
        /// Gets or sets a value indicating whether the request parameter is supported.
        /// </summary>
        [JsonPropertyName("request_parameter_supported")]
        public bool RequestParameterSupported { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Uri parameter is supported.
        /// </summary>
        [JsonPropertyName("request_uri_parameter_supported")]
        public bool RequestUriParameterSupported { get; set; } = false;

        /// <summary>
        /// Gets or sets the resource_registration_endpoint.
        /// </summary>
        [JsonPropertyName("resource_registration_endpoint")]
        public string ResourceRegistrationEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the permission_endpoint.
        /// </summary>
        [JsonPropertyName("permission_endpoint")]
        public string PermissionEndpoint { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the policy_endpoint.
        /// </summary>
        [JsonPropertyName("policy_endpoint")]
        public string PolicyEndpoint { get; set; } = string.Empty;
    }
}