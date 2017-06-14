using System.Collections.Generic;
using System.Threading.Tasks;
using Cimpress.FulfillmentLocationNetCore.Models;

namespace Cimpress.FulfillmentLocationNetCore
{
    public interface IFulfillmentLocationClient
    {
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations();
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(bool showArchived);
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(string fulfillerId);
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(string fulfillerId, bool showArchived);
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(int internalFulfillerId);
        Task<ICollection<FulfillmentLocation>> GetFulfillmentLocations(int internalFulfillerId, bool showArchived);
        Task<FulfillmentLocation> CreateFulfillmentLocation(LocationConfiguration locationConfiguration);
        Task<FulfillmentLocation> GetFulfillmentLocation(int internalFulfillmentLocationId);
        Task<FulfillmentLocation> GetFulfillmentLocation(string fulfillmentLocationId);
        Task<FulfillmentLocation> UpdateFulfillmentLocation(string fulfillmentLocationId, LocationConfiguration locationConfiguration);
        Task<FulfillmentLocation> UpdateFulfillmentLocation(int internalFulfillmentLocationId, LocationConfiguration locationConfiguration);
        Task DeleteFulfillmentLocation(string fulfillmentLocationId);
        Task DeleteFulfillmentLocation(int internalFulfillmentLocationId);
    }
}