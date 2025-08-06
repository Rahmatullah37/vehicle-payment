namespace VisualSoft.Surveillance.Payment.API.Models.Response
{
    public class TransactionReportResponseModel
    {
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }

        public string? FilteredByVehicleCategory { get; set; }
        public string? FilteredByPaymentMode { get; set; }
        public List<VehicleSummaryapi> GroupedSummaries { get; set; } = new();
    }
    public class VehicleSummaryapi
    {
        public string VehicleCategory { get; set; }
        public string PaymentMode { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
