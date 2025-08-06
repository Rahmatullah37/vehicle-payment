namespace VisualSoft.Surveillance.Payment.API.Models.UpdateRequest
{
    public class FixedTarifUpdateModel
    {

        public decimal Amount { get; set; }
        public Guid Tarif_Id { get; set; }
        public bool Is_Active { get; set; } = true;

    }
}
