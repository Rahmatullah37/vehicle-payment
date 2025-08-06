namespace VisualSoft.Surveillance.Payment.API.Models.UpdateRequest
{
    public class TarifUpdateModel
    {

        public Guid Vehicle_Type_Id { get; set; }
        public Guid Tarif_Type_Id { get; set; }
        public bool Is_Active { get; set; } = true;
        public string Description { get; set; }

    }
}
