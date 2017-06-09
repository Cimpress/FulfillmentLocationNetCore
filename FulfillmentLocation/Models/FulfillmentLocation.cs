using Newtonsoft.Json;
using System.Text;

namespace Cimpress.FulfillmentLocationNetCore.Models
{

    /// <summary>
    /// 
    /// </summary>
    public class FulfillmentLocation {
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
        public string FulfillerId { get; set; }

        /// <summary>
        /// The internal ID of this fulfiller
        /// </summary>
        /// <value>The internal ID of this fulfiller</value>
        [JsonProperty(PropertyName = "internalFulfillerId")]
        public int InternalFulfillerId { get; set; }

        /// <summary>
        /// The ID of this fulfillment location
        /// </summary>
        /// <value>The ID of this fulfillment location</value>
        [JsonProperty(PropertyName = "fulfillmentLocationId")]
        public string FulfillmentLocationId { get; set; }

        /// <summary>
        /// The internal ID of this fulfillment location
        /// </summary>
        /// <value>The internal ID of this fulfillment location</value>
        [JsonProperty(PropertyName = "internalFulfillmentLocationId")]
        public int InternalFulfillmentLocationId { get; set; }

		/// <summary>
		/// Signals if the fulfiller is inactive and has been archived
		/// </summary>
		/// <value>Signals if the fulfiller is inactive and has been archived</value>
		[JsonProperty(PropertyName = "archived")]
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
			sb.Append("class FulfillmentConfiguration {\n");
			sb.Append("  Name: ").Append(Name).Append("\n");
			sb.Append("  Archived: ").Append(Archived).Append("\n");
			sb.Append("  FulfillerId: ").Append(FulfillerId).Append("\n");
			sb.Append("  InternalFulfillerId: ").Append(InternalFulfillerId).Append("\n");
			sb.Append("  FulfillmentLocationId: ").Append(FulfillmentLocationId).Append("\n");
			sb.Append("  InternalFulfillmentLocationId: ").Append(InternalFulfillmentLocationId).Append("\n");
			sb.Append("  DispatchAddressCountry: ").Append(DispatchAddressCountry).Append("\n");
			sb.Append("  DispatchAddressPostalCode: ").Append(DispatchAddressPostalCode).Append("\n");
			sb.Append("  DispatchAddressRegion: ").Append(DispatchAddressRegion).Append("\n");
			sb.Append("  LocalOrderAcceptanceCutOffTime: ").Append(LocalOrderAcceptanceCutOffTime).Append("\n");
			sb.Append("  TimeZone: ").Append(TimeZone).Append("\n");
			sb.Append("  QuoterLocationTag: ").Append(QuoterLocationTag).Append("\n");
			sb.Append("}\n");
			return sb.ToString();
		}

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var location = obj as FulfillmentLocation;
            if (location == null)
            {
                return false;
            }

            return Name == location.Name &&
                FulfillerId == location.FulfillerId &&
                InternalFulfillerId == location.InternalFulfillerId &&
                FulfillmentLocationId == location.FulfillmentLocationId &&
                InternalFulfillmentLocationId == location.InternalFulfillmentLocationId &&
                Archived == location.Archived &&
                DispatchAddressCountry == location.DispatchAddressCountry &&
                DispatchAddressPostalCode == location.DispatchAddressPostalCode &&
                DispatchAddressRegion == location.DispatchAddressRegion &&
                LocalOrderAcceptanceCutOffTime == location.LocalOrderAcceptanceCutOffTime &&
                TimeZone == location.TimeZone &&
                QuoterLocationTag == location.QuoterLocationTag;
        }
    }
}
