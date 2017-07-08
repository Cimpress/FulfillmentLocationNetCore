using System;
using System.Text;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Cimpress.FulfillmentLocationNetCore.Models;
using Cimpress.FulfillmentLocationNetCore.Errors;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Cimpress.FulfillmentLocationNetCore
{
    public class FulfillmentLocationClient : IFulfillmentLocationClient
    {
        private readonly Uri _url;
        private readonly Uri _defaultUrl = new Uri("https://fulfillmentlocation.trdlnk.cimpress.io");
        private readonly HttpClient _httpClient;
        private readonly Func<string> _authorizationProvider;
        private static readonly IReadOnlyDictionary<string, string> _routes = new Dictionary<string, string>()
        {
            { "fulfillmentLocations", "/v1/fulfillmentlocations" },
            { "fulfillmentLocationById", "/v1/fulfillmentlocations/{id}" }
        };

        public Uri Url { get; private set; }

        public FulfillmentLocationClient(string authorization) {
            _url = _defaultUrl;
            _httpClient = new HttpClient();
            _ConfigureClient();

            _authorizationProvider = () => { return authorization; };
        }

        public FulfillmentLocationClient(Func<string> authorizationProvider) {
                _url = _defaultUrl;
                _httpClient = new HttpClient();
                _ConfigureClient();
        
                _authorizationProvider = authorizationProvider;
        }

        public FulfillmentLocationClient(Func<string> authorizationProvider, HttpMessageHandler httpMessageHandler) {
            _url = _defaultUrl;
            _httpClient = new HttpClient(httpMessageHandler);
            _ConfigureClient();

            _authorizationProvider = authorizationProvider;
        }

            public FulfillmentLocationClient(Uri url, string authorization)
        {
            _url = url;
            _httpClient = new HttpClient();
            _ConfigureClient();

            _authorizationProvider = () => { return authorization; };
        }

        public FulfillmentLocationClient(Uri url, Func<string> authorizationProvider)
        {
            _url = url;
            _httpClient = new HttpClient();
            _ConfigureClient();

            _authorizationProvider = authorizationProvider;
        }

        public FulfillmentLocationClient(Uri url, Func<string> authorizationProvider, HttpMessageHandler httpMessageHandler)
        {
            _url = url;
            _httpClient = new HttpClient(httpMessageHandler);
            _ConfigureClient();

            _authorizationProvider = authorizationProvider;
        }

        private void _ConfigureClient()
        {
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("UserAgent", "Fulfillment Location .NET Core Client by Cimpress");
        }

        private async Task<ICollection<FulfillmentLocation>> _GetFulfillmentLocations(string fulfillerId = null, bool showArchived = false)
        {
            var builder = new UriBuilder(_url);
            builder.Path += builder.Path.Last() == '/' ? _routes["fulfillmentLocations"].Substring(1) : _routes["fulfillmentLocations"];
            if (!String.IsNullOrEmpty(fulfillerId))
            {
                var param = "fulfillerId=" + fulfillerId;
                builder.Query = builder.Query.Length > 1 ? builder.Query.Substring(1) + "&" + param : param;
            }
            if (showArchived)
            {
                var param = "showArchived=true";
                builder.Query = builder.Query.Length > 1 ? builder.Query.Substring(1) + "&" + param : param;
            }
            var request = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Get,
                Headers = {
                    { "Authorization", _authorizationProvider() },
                }
            };
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new CommunicationFailureException();
            }
            else
            {
                if (response.Content == null)
                {
                    return null;
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<FulfillmentLocation>>(stringResponse);
                }
            }
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations()
        {
            return await _GetFulfillmentLocations();
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(bool showArchived)
        {
            return await _GetFulfillmentLocations(String.Empty, showArchived);
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(string fulfillerId)
        {
            return await _GetFulfillmentLocations(fulfillerId);
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(string fulfillerId, bool showArchived)
        {
            return await _GetFulfillmentLocations(fulfillerId, showArchived);
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(int internalFulfillerId)
        {
            return await _GetFulfillmentLocations(internalFulfillerId.ToString());
        }
        public async Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(int internalFulfillerId, bool showArchived)
        {
            return await _GetFulfillmentLocations(internalFulfillerId.ToString(), showArchived);
        }


        public async Task<FulfillmentLocation> CreateFulfillmentLocation(LocationConfiguration locationConfiguration)
        {
            var builder = new UriBuilder(_url);
            builder.Path += builder.Path.Last() == '/' ? _routes["fulfillmentLocations"].Substring(1) : _routes["fulfillmentLocations"];
            var request = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Post,
                Headers = {
                    { "Authorization", _authorizationProvider() },
                },
                Content = new StringContent(
                    JsonConvert.SerializeObject(locationConfiguration),
                    Encoding.UTF8,
                    "application/json")
            };
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException();
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidConfigurationException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new CommunicationFailureException();
            }
            else
            {
                if (response.Content == null)
                {
                    return null;
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FulfillmentLocation>(stringResponse);
                }
            }
        }


        private async Task<FulfillmentLocation> _GetFulfillmentLocation(string fulfillmentLocationId)
        {
            var builder = new UriBuilder(_url);
            var route = _routes["fulfillmentLocationById"].Replace("{id}", fulfillmentLocationId);
            builder.Path += builder.Path.Last() == '/' ? route.Substring(1) : route;
            var request = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Get,
                Headers = {
                    { "Authorization", _authorizationProvider() }
                }
            };
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException();
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LocationNotFoundException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new CommunicationFailureException();
            }
            else
            {
                if (response.Content == null)
                {
                    return null;
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FulfillmentLocation>(stringResponse);
                }
            }
        }
        public async Task<FulfillmentLocation> GetFulfillmentLocation(int internalFulfillmentLocationId)
        {
            return await _GetFulfillmentLocation(internalFulfillmentLocationId.ToString());
        }
        public async Task<FulfillmentLocation> GetFulfillmentLocation(string fulfillmentLocationId)
        {
            return await _GetFulfillmentLocation(fulfillmentLocationId);
        }


        private async Task<FulfillmentLocation> _UpdateFulfillmentLocation(string fulfillmentLocationId, LocationConfiguration locationConfiguration)
        {
            var builder = new UriBuilder(_url);
            var route = _routes["fulfillmentLocationById"].Replace("{id}", fulfillmentLocationId);
            builder.Path += builder.Path.Last() == '/' ? route.Substring(1) : route;
            var request = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Put,
                Headers = {
                    { "Authorization", _authorizationProvider() }
                },
                Content = new StringContent(
                    JsonConvert.SerializeObject(locationConfiguration),
                    Encoding.UTF8,
                    "application/json")
            };
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException();
            }
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                throw new InvalidConfigurationException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new CommunicationFailureException();
            }
            else
            {
                if (response.Content == null)
                {
                    return null;
                }
                else
                {
                    var stringResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<FulfillmentLocation>(stringResponse);
                }
            }
        }
        public async Task<FulfillmentLocation> UpdateFulfillmentLocation(string fulfillmentLocationId, LocationConfiguration locationConfiguration)
        {
            return await _UpdateFulfillmentLocation(fulfillmentLocationId, locationConfiguration);
        }
        public async Task<FulfillmentLocation> UpdateFulfillmentLocation(int internalFulfillmentLocationId, LocationConfiguration locationConfiguration)
        {
            return await _UpdateFulfillmentLocation(internalFulfillmentLocationId.ToString(), locationConfiguration);
        }


        private async Task _DeleteFulfillmentLocation(string fulfillmentLocationId)
        {
            var builder = new UriBuilder(_url);
            var route = _routes["fulfillmentLocationById"].Replace("{id}", fulfillmentLocationId);
            builder.Path += builder.Path.Last() == '/' ? route.Substring(1) : route;
            var request = new HttpRequestMessage()
            {
                RequestUri = builder.Uri,
                Method = HttpMethod.Delete,
                Headers = {
                    { "Authorization", _authorizationProvider() }
                }
            };
            var response = await _httpClient.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                throw new AccessForbiddenException();
            }
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new LocationNotFoundException();
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new CommunicationFailureException();
            }
            else
            {
                return;
            }
        }
        public async Task DeleteFulfillmentLocation(string fulfillmentLocationId)
        {
            await _DeleteFulfillmentLocation(fulfillmentLocationId);
        }
        public async Task DeleteFulfillmentLocation(int internalFulfillmentLocationId)
        {
            await _DeleteFulfillmentLocation(internalFulfillmentLocationId.ToString());
        }
    }
}
