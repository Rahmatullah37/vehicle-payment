namespace VehicleSurveillance.Payment.API.Models.CreateRequest
{
    public class AccessFeeTransactionCreateRequestModel
    {
        public Guid Vehicle_Id { get; set; }
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; } = null!;
        public string Payment_Mode { get; set; } = null!; 
        public bool Is_Active { get; set; } = true;
    }
}
