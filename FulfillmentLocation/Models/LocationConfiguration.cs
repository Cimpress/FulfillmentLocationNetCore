using System.Text;
using Newtonsoft.Json;

namespace Cimpress.FulfillmentLocationNetCore.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class LocationConfiguration {
        /// <summary>
        /// The name of this fulfillment location
        /// </summary>
        /// <value>The name of this fulfillment location</value>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// The ID of this fulfiller
        /// </summary>
        /// <value>The ID of this fulfiller</value>
        [JsonProperty(PropertyName = "fulfillerId")]
        [JsonConverter(typeof(FulfillerIdentifierConverter))]
        public FulfillerIdentifier FulfillerId { get; set; }

		/// <summary>
		/// Signals if the fulfiller is inactive and has been archived
		/// </summary>
		/// <value>Signals if the fulfiller is inactive and has been archived</value>
		[JsonProperty(PropertyName = "archived", NullValueHandling = NullValueHandling.Ignore)]
		public bool? Archived { get; set; }

		/// <summary>
		/// The country of the dispatch address for this fulfillment location
		/// </summary>
		/// <value>The country of the dispatch address for this fulfillment location</value>
		[JsonProperty(PropertyName = "dispatchAddressCountry")]
		public string DispatchAddressCountry { get; set; }

		/// <summary>
		/// The postal code of the dispatch address for this fulfillment location
		/// </summary>
		/// <value>The postal code of the dispatch address for this fulfillment location</value>
		[JsonProperty(PropertyName = "dispatchAddressPostalCode")]
		public string DispatchAddressPostalCode { get; set; }

		/// <summary>
		/// The region of the dispatch address for this fulfillment location
		/// </summary>
		/// <value>The region of the dispatch address for this fulfillment location</value>
		[JsonProperty(PropertyName = "dispatchAddressRegion")]
		public string DispatchAddressRegion { get; set; }

		/// <summary>
		/// The time of day at which the fulfillment location will stop taking orders for the current day
		/// </summary>
		/// <value>The time of day at which the fulfillment location will stop taking orders for the current day</value>
		[JsonProperty(PropertyName = "localOrderAcceptanceCutOffTime")]
		public string LocalOrderAcceptanceCutOffTime { get; set; }

		/// <summary>
		/// The time zone to which the fulfillment location belongs
		/// </summary>
		/// <value>The time zone to which the fulfillment location belongs</value>
		[JsonProperty(PropertyName = "timeZone")]
		public string TimeZone { get; set; }

		/// <summary>
		/// This field has been deprecated, and will be removed in the next version.
		/// </summary>
		/// <value>This field has been deprecated, and will be removed in the next version.</value>
		[JsonProperty(PropertyName = "quoterLocationTag")]
		public string QuoterLocationTag { get; set; }


		/// <summary>
		/// Get the string presentation of the object
		/// </summary>
		/// <returns>String presentation of the object</returns>
		public override string ToString()  {
			var sb = new StringBuilder();
			sb.Append("class LocationConfiguration {\n");
			sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  FulfillerId: ").Append(FulfillerId.ToString()).Append("\n");
			sb.Append("  Archived: ").Append(Archived).Append("\n");
			sb.Append("  DispatchAddressCountry: ").Append(DispatchAddressCountry).Append("\n");
			sb.Append("  DispatchAddressPostalCode: ").Append(DispatchAddressPostalCode).Append("\n");
			sb.Append("  DispatchAddressRegion: ").Append(DispatchAddressRegion).Append("\n");
			sb.Append("  LocalOrderAcceptanceCutOffTime: ").Append(LocalOrderAcceptanceCutOffTime).Append("\n");
			sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
			sb.Append("  QuoterLocationTag: ").Append(QuoterLocationTag).Append("\n");
			sb.Append("}\n");
			return sb.ToString();
		}
    }
}
