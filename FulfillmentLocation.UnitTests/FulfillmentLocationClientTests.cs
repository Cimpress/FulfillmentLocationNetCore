using System;
using System.Collections.Generic;
using Xunit;
using Cimpress.FulfillmentLocationNetCore;
using Cimpress.FulfillmentLocationNetCore.Models;
using Cimpress.FulfillmentLocationNetCore.Errors;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using System.Net;
using System.Net.Http;
using System.Linq;

namespace Cimpress.FulfillmentLocationNetCore.UnitTests
{
    public class FulfillmentLocationClientTests
    {
        private static readonly Uri _url = new Uri("https://www.example.com");
        private static readonly Func<string> _authorizationProvider = () => "Bearer xyz";

        private readonly static ICollection<FulfillmentLocation> _fulfillmentLocations = new List<FulfillmentLocation>()
        {
            new FulfillmentLocation()
            {
                Name = "Lorem Ipsum",
                FulfillerId = "z1x2c3",
                InternalFulfillerId = 121,
                FulfillmentLocationId = "a1s2d3",
                InternalFulfillmentLocationId = 12321,
                DispatchAddressCountry = "PL",
                DispatchAddressPostalCode = "61-444",
                DispatchAddressRegion = "wielkopolskie",
                LocalOrderAcceptanceCutOffTime = "15:00:00",
                TimeZone = "Europe/Warsaw",
                QuoterLocationTag = "location:blues"
            },
            new FulfillmentLocation()
            {
                Name = "Dolor sit amet",
                FulfillerId = "qwerty1",
                InternalFulfillerId = 555,
                FulfillmentLocationId = "asdfgh2",
                InternalFulfillmentLocationId = 77777,
                DispatchAddressCountry = "PL",
                DispatchAddressPostalCode = "70-101",
                DispatchAddressRegion = "zachodniopomorskie",
                LocalOrderAcceptanceCutOffTime = "17:00:00",
                TimeZone = "Europe/Warsaw",
                QuoterLocationTag = "location:blues"
            }
        };

        private readonly static ICollection<LocationConfiguration> _locationConfigurations = new List<LocationConfiguration>()
        {
            new LocationConfiguration()
            {
                Name = "Lorem Ipsum",
                FulfillerId = new FulfillerIdentifier() { FulfillerId = "z1x2c3"},
                DispatchAddressCountry = "PL",
                DispatchAddressPostalCode = "61-444",
                DispatchAddressRegion = "wielkopolskie",
                LocalOrderAcceptanceCutOffTime = "15:00:00",
                TimeZone = "Europe/Warsaw",
                QuoterLocationTag = "location:blues"
            }
        };
        private static IEnumerable<object[]> GetClient(IReadOnlyDictionary<string, object> serviceResponses) {
            var httpMessageHandler = new MockHttpMessageHandler();
            foreach (var key in serviceResponses.Keys)
            {
                if (serviceResponses[key] is HttpStatusCode)
                {
                    httpMessageHandler.When(key).Respond((HttpStatusCode) serviceResponses[key]);
                }
                else
                {
                    var split = key.Split(' ');
                    var method = new HttpMethod(split[0]);
                    var request = httpMessageHandler.When(method, split[1]);
                    if (key.IndexOf('?') != -1) {
                        request = request.WithQueryString(key.Substring(key.IndexOf("?")));
                    }
                    request.Respond("application/json", serviceResponses[key] as string);
                }
            }
            yield return new object[] {
                new FulfillmentLocationClient(_url, _authorizationProvider, httpMessageHandler)
            };
        }

        private static readonly IReadOnlyDictionary<string, object> _successfulServiceResponses = new Dictionary<string, object>()
        {
            { "GET /v1/fulfillmentlocations?fulfillerId=121", JsonConvert.SerializeObject(new FulfillmentLocation[] { _fulfillmentLocations.ElementAt(0) }) },
            { "GET /v1/fulfillmentlocations?fulfillerId=z1x2c3", JsonConvert.SerializeObject(new FulfillmentLocation[] { _fulfillmentLocations.ElementAt(0) }) },
            { "GET /v1/fulfillmentlocations/12321", JsonConvert.SerializeObject(_fulfillmentLocations.ElementAt(0)) },
            { "GET /v1/fulfillmentlocations/a1s2d3", JsonConvert.SerializeObject(_fulfillmentLocations.ElementAt(0)) },
            { "GET /v1/fulfillmentlocations", JsonConvert.SerializeObject(_fulfillmentLocations) },

            { "POST /v1/fulfillmentlocations", JsonConvert.SerializeObject(_fulfillmentLocations.ElementAt(0)) },

            { "PUT /v1/fulfillmentlocations/a1s2d3", JsonConvert.SerializeObject(_fulfillmentLocations.ElementAt(0)) },

            { "DELETE /v1/fulfillmentlocations/a1s2d3", null },
        };

        public static IEnumerable<object[]> GetClient(HttpStatusCode httpStatusCode)
        {
            return GetClient(new Dictionary<string, object>()
            {
                { "*", httpStatusCode }
            });
        }
        public static IEnumerable<object[]> GetSuccessfulClient()
        {
            return GetClient(_successfulServiceResponses);
        }
        public static IEnumerable<object[]> GetForbiddenClient()
        {
            return GetClient(HttpStatusCode.Forbidden);
        }
        public static IEnumerable<object[]> GetInternalServerErrorClient()
        {
            return GetClient(HttpStatusCode.InternalServerError);
        }


        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetSuccessfulClient))]
        public async void GetFulfillmentLocations_ReturnsFulfillmentLocations(FulfillmentLocationClient client) {
            var fulfillmentLocations = await client.GetFulfillmentLocations();

            Assert.True(fulfillmentLocations.SequenceEqual(_fulfillmentLocations));
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetSuccessfulClient))]
        public async void GetFulfillmentLocations_FiltersByFulfillerId(FulfillmentLocationClient client) {
            var fulfillmentLocations = await client.GetFulfillmentLocations(_fulfillmentLocations.ElementAt(0).FulfillerId);

            Assert.True(fulfillmentLocations.SequenceEqual(_fulfillmentLocations.Where(fl => fl.FulfillerId == _fulfillmentLocations.ElementAt(0).FulfillerId)));
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetSuccessfulClient))]
        public async void GetFulfillmentLocations_FiltersByInternalFulfillerId(FulfillmentLocationClient client) {
            var fulfillmentLocations = await client.GetFulfillmentLocations(_fulfillmentLocations.ElementAt(0).InternalFulfillerId);

            Assert.True(fulfillmentLocations.SequenceEqual(_fulfillmentLocations.Where(fl => fl.FulfillerId == _fulfillmentLocations.ElementAt(0).FulfillerId)));
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.Forbidden)]
        public async void GetFulfillmentLocations_ThrowsCorrectExceptionOnForbidden(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<AccessForbiddenException>(async () => {
                await client.GetFulfillmentLocations();
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.InternalServerError)]
        public async void GetFulfillmentLocations_ThrowsCorrectExceptionOnOtherError(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<CommunicationFailureException>(async () => {
                await client.GetFulfillmentLocations();
            });
        }


        [Theory]
        [MemberData(nameof(GetSuccessfulClient))]
        public async void GetFulfillmentLocation_ReturnsFulfillmentLocationByFulfillmentLocationId(FulfillmentLocationClient client)
        {
            var fulfillmentLocation = await client.GetFulfillmentLocation(_fulfillmentLocations.ElementAt(0).FulfillmentLocationId);

            Assert.Equal(_fulfillmentLocations.ElementAt(0), fulfillmentLocation);
        }
        public async void GetFulfillmentLocation_ReturnsFulfillmentLocationByInternalFulfillmentLocationId(FulfillmentLocationClient client)
        {
            var fulfillmentLocation = await client.GetFulfillmentLocation(_fulfillmentLocations.ElementAt(0).InternalFulfillmentLocationId);

            Assert.Equal(_fulfillmentLocations.ElementAt(0), fulfillmentLocation);
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.Forbidden)]
        public async void GetFulfillmentLocation_ThrowsCorrectExceptionOnForbidden(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<AccessForbiddenException>(async () => {
                await client.GetFulfillmentLocation(String.Empty);
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.NotFound)]
        public async void GetFulfillmentLocation_ThrowsCorrectExceptionOnNotFound(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<LocationNotFoundException>(async () => {
                await client.GetFulfillmentLocation(String.Empty);
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.InternalServerError)]
        public async void GetFulfillmentLocation_ThrowsCorrectExceptionOnOtherError(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<CommunicationFailureException>(async () => {
                await client.GetFulfillmentLocation(String.Empty);
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.Forbidden)]
        public async void CreateFulfillmentLocation_ThrowsCorrectExceptionOnForbidden(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<AccessForbiddenException>(async () => {
                await client.CreateFulfillmentLocation(_locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.BadRequest)]
        public async void CreateFulfillmentLocation_ThrowsCorrectExceptionOnBadRequest(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<InvalidConfigurationException>(async () => {
                await client.CreateFulfillmentLocation(_locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.InternalServerError)]
        public async void CreateFulfillmentLocation_ThrowsCorrectExceptionOnOtherError(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<CommunicationFailureException>(async () => {
                await client.CreateFulfillmentLocation(_locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.Forbidden)]
        public async void UpdateFulfillmentLocation_ThrowsCorrectExceptionOnForbidden(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<AccessForbiddenException>(async () => {
                await client.UpdateFulfillmentLocation("a1s2d3", _locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.BadRequest)]
        public async void UpdateFulfillmentLocation_ThrowsCorrectExceptionOnBadRequest(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<InvalidConfigurationException>(async () => {
                await client.UpdateFulfillmentLocation("a1s2d3", _locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.InternalServerError)]
        public async void UpdateFulfillmentLocation_ThrowsCorrectExceptionOnOtherError(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<CommunicationFailureException>(async () => {
                await client.UpdateFulfillmentLocation("a1s2d3", _locationConfigurations.ElementAt(0));
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.Forbidden)]
        public async void DeleteFulfillmentLocation_ThrowsCorrectExceptionOnForbidden(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<AccessForbiddenException>(async () => {
                await client.DeleteFulfillmentLocation("a1s2d3");
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.NotFound)]
        public async void DeleteFulfillmentLocation_ThrowsCorrectExceptionOnNotFound(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<LocationNotFoundException>(async () => {
                await client.GetFulfillmentLocation(String.Empty);
            });
        }

        [Theory]
        [MemberData(nameof(FulfillmentLocationClientTests.GetClient), HttpStatusCode.InternalServerError)]
        public async void DeleteFulfillmentLocation_ThrowsCorrectExceptionOnOtherError(FulfillmentLocationClient client)
        {
            await Assert.ThrowsAsync<CommunicationFailureException>(async () => {
                await client.DeleteFulfillmentLocation("a1s2d3");
            });
        }
    }
}
