using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Cimpress.FulfillmentLocationNetCore.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Reflection;

namespace Cimpress.FulfillmentLocationNetCore.Tests
{
    public class LocationConfigurationTests
    {
        public static IEnumerable<object[]> GenerateFullLocationConfiguration() {

            yield return new object[] {
                new LocationConfiguration() {
                    Name = "Lorem Ipsum",
                    FulfillerId = new FulfillerIdentifier() { FulfillerId = "z1x2c3" },
                    Archived = false,
                    DispatchAddressCountry = "PL",
                    DispatchAddressPostalCode = "61-444",
                    DispatchAddressRegion = "Wielkopolska",
                    LocalOrderAcceptanceCutOffTime = "15:00:00",
                    TimeZone = "Europe/Warsaw",
                    QuoterLocationTag = "location:blues"
                }
            };

            yield return new object[] {
                new LocationConfiguration() {
                    Name = "Dolor sit amet",
                    FulfillerId = new FulfillerIdentifier() { InternalFulfillerId = 121},
                    Archived = true,
                    DispatchAddressCountry = "PL",
                    DispatchAddressPostalCode = "61-444",
                    DispatchAddressRegion = "Wielkopolska",
                    LocalOrderAcceptanceCutOffTime = "15:00:00",
                    TimeZone = "Europe/Warsaw",
                    QuoterLocationTag = "location:blues"
                }
            };
        }

        public static IEnumerable<object[]> GenerateSerializedLocationConfiguration()
        {
            yield return new object[] {
                "{\"name\":\"consectetur\",\"fulfillerId\":\"a1b2c3\",\"archived\":false,\"dispatchAddressCountry\":\"PL\",\"dispatchAddressPostalCode\":\"61-444\",\"dispatchAddressRegion\":\"Wielkopolska\",\"localOrderAcceptanceCutOffTime\":\"15:00:00\",\"timeZone\":\"Europe/Warsaw\",\"quoterLocationTag\":\"location:consectetur\"}"
            };
        }

        public void CompareJsonAndNetObject(JObject json, LocationConfiguration configuration)
        {
            foreach (var propertyType in typeof(LocationConfiguration).GetProperties().Where(p => p.Name != "FulfillerId"))
            {
                var jsonName = Char.ToLower(propertyType.Name[0]) + propertyType.Name.Substring(1);
                Assert.Equal(propertyType.GetValue(configuration).ToString(), json.GetValue(jsonName).ToString());
            }
            if (configuration.FulfillerId.InternalFulfillerId.HasValue)
            {
                Assert.Equal(configuration.FulfillerId.InternalFulfillerId.Value, json.GetValue("fulfillerId"));
            }
            else
            {
                Assert.Equal(configuration.FulfillerId.FulfillerId, json.GetValue("fulfillerId").ToString());
            }
        }

        [Theory]
        [MemberData(nameof(GenerateFullLocationConfiguration))]
        public void SerializesProperly(LocationConfiguration configuration)
        {
            var serialized = JsonConvert.SerializeObject(configuration);
            var json = JObject.Parse(serialized);

            CompareJsonAndNetObject(json, configuration);
        }

        [Theory]
        [MemberData(nameof(GenerateSerializedLocationConfiguration))]
        public void DeserializesProperly(string serialized)
        {
            var configuration = JsonConvert.DeserializeObject<LocationConfiguration>(serialized);
            var json = JObject.Parse(serialized);

            CompareJsonAndNetObject(json, configuration);
        }
    }
}
