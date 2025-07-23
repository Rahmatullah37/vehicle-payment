namespace VehicleSurveillance.Payment.API.Models.CreateRequest
{
    public class HourlyTarifRequestModel
    {
        public string VehicleTypeName { get; set; }
        public DateTime FromTime { get; set; }   // ← datetime
        public DateTime ToTime { get; set; }     // ← datetime
        public string PaymentMode { get; set; }
    }
}
