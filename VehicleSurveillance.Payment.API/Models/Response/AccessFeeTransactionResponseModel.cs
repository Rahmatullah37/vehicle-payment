namespace VisualSoft.Surveillance.Payment.API.Models.Response
{
    public class AccessFeeTransactionResponseModel
    {
        public Guid Id { get; set; }
        public Guid Vehicle_Id { get; set; }
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; }
        public string Payment_Mode { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }


    }
}
