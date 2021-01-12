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
namespace Keycloak.Client.Resource
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Keycloak.Authorization.Representation;
    using Keycloak.Client.Services;
    using Keycloak.Client.Util;

    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    ///
    /// <summary>An entry point for managing permission tickets using the Protection API.</summary>
    ///
    public class ProtectedResource : IProtectedResource
    {
        private readonly ILogger logger;

        private readonly IHttpClientService httpClientService;

        private readonly IServerConfigurationResource serverConfigurationDelegate;

        /// <summary>Initializes a new instance of the <see cref="ProtectedResource"/> class.</summary>
        /// <param name="logger">The injected logger provider.</param>
        /// <param name="httpClientService">injected HTTP client service.</param>
        /// <param name="serverConfigurationDelegate">The injected UMA 2 server-side configuration delegate.</param>
        public ProtectedResource(
            ILogger<PermissionResource> logger,
            IServerConfigurationResource serverConfigurationDelegate,
            IHttpClientService httpClientService)
        {
            this.logger = logger;
            this.serverConfigurationDelegate = serverConfigurationDelegate;
            this.httpClientService = httpClientService;
        }

        /// <inheritdoc/>
        public async Task<Resource> Create(Resource resource, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.ResourceRegistrationEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<Resource>(resource);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogError($"create() returned with StatusCode := {response.StatusCode}.");
                }

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                Resource ?resourceResponse = JsonSerializer.Deserialize<Resource>(result);
                return resourceResponse!;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> Update(Resource resource, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/" + resource.Id;
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<Resource>(resource);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PutAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    string msg = $"update() returned with StatusCode := {response.StatusCode}.";
                    this.logger.LogError(msg);
                    throw new HttpRequestException(msg);
                }

                return true;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> Delete(string resourceId, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.ResourceRegistrationEndpoint + "/" + resourceId;
            client.BaseAddress = new Uri(requestUrl);

            HttpResponseMessage response = await client.DeleteAsync(new Uri(requestUrl)).ConfigureAwait(true);
            if (!response.IsSuccessStatusCode)
            {
                string msg = $"delete() returned with StatusCode := {response.StatusCode}.";
                this.logger.LogError(msg);
                throw new HttpRequestException(msg);
            }

            return true;
        }

        /// <inheritdoc/>
        public async Task<Resource> FindById(string resourceId, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.ResourceRegistrationEndpoint + "/" + resourceId;
            client.BaseAddress = new Uri(requestUrl);

            HttpResponseMessage response = await client.GetAsync(new Uri(requestUrl)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"findById() returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Resource ?resourceResponse = JsonSerializer.Deserialize<Resource>(result);
            return resourceResponse!;
        }

        /// <inheritdoc/>
        public async Task<List<Resource>> FindByUri(Uri uri, string token)
        {
            return await this.Find(null, null, uri, null, null, null, false, false, true, null, null, token).ConfigureAwait(true);
        }

        /// <inheritdoc/>
        public async Task<List<Resource>> FindByMatchingUri(Uri uri, string token)
        {
            return await this.Find(null, null, uri, null, null, null, true, false, true, null, null, token).ConfigureAwait(true);
        }

        /// <inheritdoc/>
        public async Task<string[]> FindAll(string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.ResourceRegistrationEndpoint;
            client.BaseAddress = new Uri(requestUrl);
            requestUrl = QueryHelpers.AddQueryString(requestUrl, "deep", "false");

            HttpResponseMessage response = await client.GetAsync(new Uri(requestUrl)).ConfigureAwait(true);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"findAll() returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            string[] ?resourceIds = JsonSerializer.Deserialize<string[]>(result);
            return resourceIds!;
        }

        /// <summary>
        /// Query the server for a Resource with a given Uri.
        /// This method queries the server for resources who matches the parameters.
        /// </summary>
        /// <param name="resourceId">The resource ID.</param>
        /// <param name="name">The resource name.</param>
        /// <param name="uri">The resource uri.</param>
        /// <param name="owner">The resource owner.</param>
        /// <param name="type">The resource type.</param>
        /// <param name="scope">The resource scope.</param>
        /// <param name="matchingUri">Boolean to use best matching for Uri.</param>
        /// <param name="exactName">Boolean to indicate exact matching on name.</param>
        /// <param name="deep">Boolean to use deep matching.</param>
        /// <param name="firstResult">first Result index.</param>
        /// <param name="maxResult">Max Result.  -1 for no limit.</param>
        /// <param name="token"> A valid base64 access_token from authenticing the caller.</param>
        /// <returns>Returns a list of Resources that best matches the given Uri.</returns>
        private async Task<List<Resource>> Find(
                string? resourceId,
                string? name,
                Uri? uri,
                string? owner,
                string? type,
                string? scope,
                bool matchingUri,
                bool exactName,
                bool deep,
                int? firstResult,
                int? maxResult,
                string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.ResourceRegistrationEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            if (resourceId != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "_id", resourceId);
            }

            if (name != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "name", name);
            }

            if (uri != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "uri", uri.AbsoluteUri);
            }

            if (owner != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "owner", owner);
            }

            if (type != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "type", type);
            }

            if (scope != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "scope", scope);
            }

            requestUrl = QueryHelpers.AddQueryString(requestUrl, "matchingUri", matchingUri.ToString());
            requestUrl = QueryHelpers.AddQueryString(requestUrl, "exactName", exactName.ToString());
            requestUrl = QueryHelpers.AddQueryString(requestUrl, "deep", deep.ToString());

            if (firstResult != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "first", firstResult.ToString());
            }

            if (maxResult != null)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "max", maxResult.ToString());
            }
            else
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, "max", "-1");
            }

            HttpResponseMessage response = await client.GetAsync(new Uri(requestUrl)).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"find() returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            List<Resource> ?resourceResponse = JsonSerializer.Deserialize<List<Resource>>(result);
            return resourceResponse!;
        }
    }
}