using System;
using Xunit;
using Cimpress.FulfillmentLocationNetCore;
using Cimpress.FulfillmentLocationNetCore.Models;
using System.Collections.Generic;

namespace Cimpress.FulfillmentLocationNetCore.IntegrationTests
{
    public class FulfillmentLocationClientTests
    {
        private FulfillmentLocationClient _client;

        private static readonly string _authorization = System.Environment.GetEnvironmentVariable("CIMPRESS_FULFILLMENT_LOCATION_BEARER_TOKEN");
        private static readonly string _url = System.Environment.GetEnvironmentVariable("CIMPRESS_FULFILLMENT_LOCATION_SERVICE_URL");
        private static readonly string _testFulfillerId = System.Environment.GetEnvironmentVariable("CIMPRESS_TEST_FULFILLER_ID");


        private static readonly LocationConfiguration _locationConfiguration = new LocationConfiguration()
        {
                Name = "TEST - Cimpress.FulfillmentLocationNetCore",
                FulfillerId = new FulfillerIdentifier() { FulfillerId = _testFulfillerId },
                DispatchAddressCountry = "PL",
                DispatchAddressPostalCode = "61-444",
                DispatchAddressRegion = "wielkopolskie",
                LocalOrderAcceptanceCutOffTime = "15:00:00",
                TimeZone = "Europe/Warsaw",
                QuoterLocationTag = "location:test"
        };

        public FulfillmentLocationClientTests()
        {
            if (String.IsNullOrEmpty(_authorization) || String.IsNullOrEmpty(_url) || String.IsNullOrEmpty(_testFulfillerId))
            {
                throw new Exception("Please set up test variables in your environment.");
            }
            _client = new FulfillmentLocationClient(new Uri(_url), _authorization);
        }

        [Fact]
        public async void GetFulfillmentLocations_RetrievesAllFulfillmentLocations()
        {
            var fulfillmentLocations = await _client.GetFulfillmentLocations();
            Console.WriteLine("Number of locations retrieved: {0}", fulfillmentLocations.Count);
        }

        [Fact]
        public async void _ClientV1_SupportsAllOperationsOnFulfillmentLocation()
        {
            // CreateFulfillmentLocation
            var fulfillmentLocation = await _client.CreateFulfillmentLocation(_locationConfiguration);
            var createdFulfillmentLocationId = fulfillmentLocation.FulfillmentLocationId;

            // GetFulfillmentLocation
            fulfillmentLocation = await _client.GetFulfillmentLocation(createdFulfillmentLocationId);
            Console.WriteLine("Posted test location:\n {0}", fulfillmentLocation.ToString());

            // UpdateFulfillmentLocation
            fulfillmentLocation = await _client.UpdateFulfillmentLocation(createdFulfillmentLocationId, _locationConfiguration);
            Console.WriteLine("Updated test location:\n {0}", fulfillmentLocation.ToString());

            // DeleteFulfillmentLocation
            await _client.DeleteFulfillmentLocation(createdFulfillmentLocationId);
        }
    }
}
