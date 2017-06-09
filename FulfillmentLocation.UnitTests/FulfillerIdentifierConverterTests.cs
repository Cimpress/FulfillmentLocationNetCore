using Xunit;
using Cimpress.FulfillmentLocationNetCore.Models;
using Newtonsoft.Json;

namespace Cimpress.FulfillmentLocationNetCore.Tests
{
    public class FulfillerIdentifierConverterSerializerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(121)]
        public void SerializesNumericalFulfillerId(int testId)
        {
            var fulfillerId = new FulfillerIdentifier();
            fulfillerId.InternalFulfillerId = testId;
            var serialized = JsonConvert.SerializeObject(fulfillerId, new JsonConverter[] { new FulfillerIdentifierConverter() });
            Assert.Equal(JsonConvert.SerializeObject(testId), serialized);
        }

        [Theory]
        [InlineData("123ac")]
        [InlineData("112dsa")]
        [InlineData("hmoig09")]
        public void SerializesAlphanumericalFulfillerId(string testId)
        {
            var fulfillerId = new FulfillerIdentifier();
            fulfillerId.FulfillerId = testId;
            var serialized = JsonConvert.SerializeObject(fulfillerId, new JsonConverter[] { new FulfillerIdentifierConverter() });
            Assert.Equal(JsonConvert.SerializeObject(testId), serialized);
        }
    }

    public class FulfillerIdentifierConverterDeserializerTests
    {
        [Theory]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(121)]
        public void DeserializesFromNumericalFulfillerId(int testId)
        {
            var serialized = JsonConvert.SerializeObject(testId);            
            var fulfillerId = JsonConvert.DeserializeObject<FulfillerIdentifier>(serialized, new JsonConverter[] { new FulfillerIdentifierConverter() });
            Assert.Equal(fulfillerId.InternalFulfillerId, testId);
        }

        [Theory]
        [InlineData("123ac")]
        [InlineData("112dsa")]
        [InlineData("hmoig09")]
        public void DeserializesFromAlphaNumericalFulfillerId(string testId)
        {
            var serialized = JsonConvert.SerializeObject(testId);            
            var fulfillerId = JsonConvert.DeserializeObject<FulfillerIdentifier>(serialized, new JsonConverter[] { new FulfillerIdentifierConverter() });
            Assert.Equal(fulfillerId.FulfillerId, testId);
        }
    }
}
