namespace VehicleSurveillance.Payment.API.Models.CreateRequest
{
    public class PackageCreateRequestModel
    {

        public string Package_Type { get; set; }
        public decimal Package_Cost { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
