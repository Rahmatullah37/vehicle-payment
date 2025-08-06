namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class VehicleTarifModel
    {
        public string VehicleType { get; set; }
        public decimal Amount { get; set; }
        public float totalhours { get; set; }
        public string TotalDuration { get; set; }
        public bool IsPackageTransaction { get; set; }
        public string Message { get; set; }


    }
}
