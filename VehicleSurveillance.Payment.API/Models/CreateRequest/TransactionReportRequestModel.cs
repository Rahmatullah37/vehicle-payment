namespace VisualSoft.Surveillance.Payment.API.Models.CreateRequest
{
    public class TransactionReportRequestModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? VehicleCategory { get; set; }
        public string? PaymentMode { get; set; }
    }
}
