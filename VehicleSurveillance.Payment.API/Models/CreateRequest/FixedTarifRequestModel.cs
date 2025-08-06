namespace VisualSoft.Surveillance.Payment.API.Models.CreateRequest
{
    public class FixedTarifRequestModel
    {

        public decimal Amount { get; set; }
        public Guid Tarif_Id { get; set; }
        public bool Is_Active { get; set; } = true;

    }
}
