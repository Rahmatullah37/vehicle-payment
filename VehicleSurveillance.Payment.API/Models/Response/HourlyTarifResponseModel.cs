namespace VehicleSurveillance.Payment.API.Models.Response
{
    public class HourlyTarifResponseModel
    {
        public Guid Id { get; set; }
        public int FromHour { get; set; }
        public int ToHour { get; set; }
        public decimal Amount { get; set; }
        public decimal totalhours { get; set; }
        public Guid Tarif_Id { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
