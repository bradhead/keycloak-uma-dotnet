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
namespace Keycloak.Client.Util
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;

    using Keycloak.Authorization;
    using Keycloak.Authorization.Representation;
    using Keycloak.Authorization.Representation.Tokens;

    /// <summary>Extensions for HttpClient to handle OAuth 2.0 UMA.</summary>
    public static class HttpClientExtensions
    {
        /// <summary>Sets the Bearer Token Head for UMA calls.</summary>
        /// <param name="httpClient">The target of this extension method.</param>
        /// <param name="base64BearerToken">The OAuth 2 Access Token base 64 encoded.</param>
        public static void BearerTokenAuthorization(this HttpClient httpClient, string base64BearerToken)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", base64BearerToken);
        }

        /// <summary>Sets the UMA multipart form parameters from the AuthorizationRequest provided and posts the request.</summary>
        /// <param name="httpClient">The target of this extension method.</param>
        /// <param name="uri">The <see cref="Uri"/> endpoint to post to.</param>
        /// <param name="request">An <see cref="AuthorizationRequest"/> request.</param>
        /// <returns>An HttpResponseMessage.</returns>
        public static Task<HttpResponseMessage> PostUmaAsync(this HttpClient httpClient, Uri uri, AuthorizationRequest request)
        {
            string? ticket = request.Ticket;
            PermissionTicketToken? permissionTicketToken = request.Permissions;

            if (ticket == null && permissionTicketToken == null)
            {
                throw new ArgumentException("You must either provide a permission ticket or the permissions you want to request.");
            }

            Dictionary<string, string> paramDict = new Dictionary<string, string>();
            paramDict.Add(OAuth2Constants.UmaGrantType, OAuth2Constants.GrantType);
            paramDict.Add("ticket", ticket!);
            paramDict.Add("claim_token", request.ClaimToken!);
            paramDict.Add("claim_token_format", request.ClaimTokenFormat!);
            paramDict.Add("pct", request.Pct!);
            paramDict.Add("rpt", request.RptToken!);
            paramDict.Add("scope", request.Scope!);
            paramDict.Add("audience", request.Audience!);
            paramDict.Add("subject_token", request.SubjectToken!);

            if (permissionTicketToken!.Permissions != null)
            {
                foreach (Permission permission in permissionTicketToken.Permissions)
                {
                    string resourceId = permission.ResourceId;
                    List<string>? scopes = permission.Scopes;
                    StringBuilder value = new StringBuilder();

                    if (resourceId != null)
                    {
                        value.Append(resourceId);
                    }

                    if (scopes != null && (scopes.Count > 0))
                    {
                        value.Append('#');
                        foreach (string scope in scopes)
                        {
                            string val = value.ToString();
                            if (!val.EndsWith('#'))
                            {
                                value.Append(',');
                            }

                            value.Append(scope);
                        }
                    }

                    paramDict.Add(value.ToString(), "permission");
                }
            }

            RequestMetadata? metadata = request.Metadata;

            if (metadata != null)
            {
                if (metadata.IncludeResourceName)
                {
                    paramDict.Add(metadata.IncludeResourceName.ToString(CultureInfo.InvariantCulture), "response_include_resource_name");
                }

                if (metadata.Limit > 0)
                {
                    paramDict.Add(metadata.Limit.ToString(CultureInfo.InvariantCulture), "response_permissions_limit");
                }
            }

            using (HttpContent ?content = new FormUrlEncodedContent(paramDict!))
            {
                content.Headers.Add(@"Content-Type", @"application/x-www-form-urlencoded");
                return httpClient.PostAsync(uri, content);
            }
        }
    }
}