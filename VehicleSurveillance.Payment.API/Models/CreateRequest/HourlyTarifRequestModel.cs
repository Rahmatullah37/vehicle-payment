using Azure.Core;

namespace VisualSoft.Surveillance.Payment.API.Models.CreateRequest
{
    public class HourlyTarifRequestModel
    {
        public string VehicleTypeName { get; set; }
        public DateTime FromTime { get; set; }   // ← datetime
        public DateTime ToTime { get; set; } 
        public Guid VehicleId { get; set; }
    }
}
