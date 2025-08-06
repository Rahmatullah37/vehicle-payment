namespace VisualSoft.Surveillance.Payment.API.Models.UpdateRequest
{
    public class AccessFeeTransactionUpdateModel
    {
        public Guid Vehicle_Id { get; set; }
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; }
        public string Payment_Mode { get; set; }  // this filed should be cash, Package, Tag, VehicleAccount
        public bool Is_Active { get; set; } = true;
    }
}
