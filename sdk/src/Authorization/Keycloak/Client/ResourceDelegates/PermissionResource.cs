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

    using Keycloak.Client.Util;
    using Keycloak.Representation;
    using Keycloak.Services;

    using Microsoft.Extensions.Logging;

    ///
    /// <summary>An entry point for managing permission tickets using the Protection API.</summary>
    ///
    public class PermissionResource : IPermissionResource
    {
        private readonly ILogger logger;

        private readonly IServerConfigurationResource serverConfigurationDelegate;

        /// <summary>The injected HttpClientService.</summary>
        private readonly IHttpClientService httpClientService;

        /// <summary>Initializes a new instance of the <see cref="PermissionResource"/> class.</summary>
        /// <param name="logger">The injected logger provider.</param>
        /// <param name="httpClientService">injected HTTP client service.</param>
        /// <param name="serverConfigurationDelegate">The keycloak UMA configuration delegate.</param>
        public PermissionResource(
            ILogger<PermissionResource> logger,
            IServerConfigurationResource serverConfigurationDelegate,
            IHttpClientService httpClientService)
        {
            this.logger = logger;
            this.serverConfigurationDelegate = serverConfigurationDelegate;
            this.httpClientService = httpClientService;
        }

        /// <inheritdoc/>
        public async Task<PermissionResponse> Create(PermissionRequest request, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<PermissionRequest>(request);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogError($"createPermissionTicket(PermissionRequest ) returned with StatusCode := {response.StatusCode}.");
                }

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                PermissionResponse permissionResponse = JsonSerializer.Deserialize<PermissionResponse>(result);
                return permissionResponse;
            }
        }

        /// <inheritdoc/>
        public async Task<PermissionResponse> Create(List<PermissionRequest> requests, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<List<PermissionRequest>>(requests);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogError($"createPermissionTicket(List<PermissionRequest> ) returned with StatusCode := {response.StatusCode}.");
                }

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                PermissionResponse permissionResponse = JsonSerializer.Deserialize<PermissionResponse>(result);
                return permissionResponse;
            }
        }

        /// <inheritdoc/>
        public async Task<PermissionTicket> Create(PermissionTicket ticket, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint;
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<PermissionTicket>(ticket);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PostAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    this.logger.LogError($"createPermissionTicket(PermissionTicket ) returned with StatusCode := {response.StatusCode}.");
                }

                string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                PermissionTicket permissionResponse = JsonSerializer.Deserialize<PermissionTicket>(result);
                return permissionResponse;
            }
        }

        /// <inheritdoc/>
        public async Task<List<PermissionTicket>> FindByScope(string scopeId, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/ticket";
            client.BaseAddress = new Uri(requestUrl);

            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = "scopeId=" + scopeId;

            HttpResponseMessage response = await client.GetAsync(builder.Uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"createPermissionTicket(PermissionTicket ) returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            List<PermissionTicket> permissionTickets = JsonSerializer.Deserialize<List<PermissionTicket>>(result);
            return permissionTickets;
        }

        /// <inheritdoc/>
        public async Task<List<PermissionTicket>> FindByResourceId(string resourceId, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/ticket";
            client.BaseAddress = new Uri(requestUrl);

            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = "resourceId=" + resourceId;

            HttpResponseMessage response = await client.GetAsync(builder.Uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"createPermissionTicket(PermissionTicket ) returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            List<PermissionTicket> permissionTickets = JsonSerializer.Deserialize<List<PermissionTicket>>(result);
            return permissionTickets;
        }

        /// <inheritdoc/>
        public async Task<List<PermissionTicket>> Find(
            string resourceId,
            string scopeId,
            string owner,
            string requester,
            bool granted,
            bool returnNames,
            int firstResult,
            int maxResult,
            string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/ticket";
            client.BaseAddress = new Uri(requestUrl);

            UriBuilder builder = new UriBuilder(requestUrl);
            builder.Query = "resourceId=" + resourceId +
                "&scopeId" + scopeId +
                "&owner" + owner +
                "&requester" + requester +
                "&granted" + granted.ToString() +
                "&returnNames" + returnNames.ToString() +
                "&first" + firstResult +
                "&max" + maxResult;

            HttpResponseMessage response = await client.GetAsync(builder.Uri).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                this.logger.LogError($"createPermissionTicket(PermissionTicket ) returned with StatusCode := {response.StatusCode}.");
            }

            string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            List<PermissionTicket> permissionTickets = JsonSerializer.Deserialize<List<PermissionTicket>>(result);
            return permissionTickets;
        }

        /// <inheritdoc/>
        public async Task<bool> Update(PermissionTicket ticket, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/ticket";
            client.BaseAddress = new Uri(requestUrl);

            string jsonOutput = JsonSerializer.Serialize<PermissionTicket>(ticket);

            using (HttpContent content = new StringContent(jsonOutput))
            {
                HttpResponseMessage response = await client.PutAsync(new Uri(requestUrl), content).ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    string msg = $"updatePermissionTicket() returned with StatusCode := {response.StatusCode}.";
                    this.logger.LogError(msg);
                    throw new HttpRequestException(msg);
                }

                return true;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> Delete(string ticketId, string token)
        {
            HttpClient client = this.httpClientService.CreateDefaultHttpClient();

            client.DefaultRequestHeaders.Accept.Clear();

            client.BearerTokenAuthorization(token);
            string requestUrl = this.serverConfigurationDelegate.ServerConfiguration.PermissionEndpoint + "/ticket/" + ticketId;
            client.BaseAddress = new Uri(requestUrl);

            HttpResponseMessage response = await client.DeleteAsync(new Uri(requestUrl)).ConfigureAwait(true);
            if (!response.IsSuccessStatusCode)
            {
                string msg = $"deletePermissionTicket() returned with StatusCode := {response.StatusCode}.";
                this.logger.LogError(msg);
                throw new HttpRequestException(msg);
            }

            return true;
        }
    }
}