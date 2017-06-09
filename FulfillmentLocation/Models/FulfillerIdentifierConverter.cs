using System;
using Newtonsoft.Json;

namespace Cimpress.FulfillmentLocationNetCore.Models
{
    public class FulfillerIdentifierConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var fulfillerIdentifier = value as FulfillerIdentifier;

            if (fulfillerIdentifier != null) {
                if (!String.IsNullOrEmpty(fulfillerIdentifier.FulfillerId))
                {
                    serializer.Serialize(writer, fulfillerIdentifier.FulfillerId);
                }
                else
                {
                    serializer.Serialize(writer, fulfillerIdentifier.InternalFulfillerId);
                }
            }
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var fulfillerIdentifier = new FulfillerIdentifier();
            switch (Type.GetTypeCode(reader.ValueType))
            {
                case TypeCode.String:
                    fulfillerIdentifier.FulfillerId = (String)serializer.Deserialize(reader);
                    break;

                case TypeCode.Int64:
                    try
                    {
                        fulfillerIdentifier.InternalFulfillerId = Convert.ToInt32(serializer.Deserialize(reader));
                    }
                    catch
                    {
                        throw new JsonSerializationException("The fulfiller ID value is too big.");
                    }
                    break;

                default:
                    throw new JsonSerializationException("The type of value is unrecognized.");
            }
            return fulfillerIdentifier;
        }


        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(FulfillerIdentifier);
        }
    }
}
