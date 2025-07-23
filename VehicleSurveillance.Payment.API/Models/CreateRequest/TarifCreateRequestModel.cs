namespace VehicleSurveillance.Payment.API.Models.CreateRequest
{
    public class TarifCreateRequestModel
    {

        public Guid Vehicle_Type_Id { get; set; }
        public Guid Tarif_Type_Id { get; set; }
        public bool Is_Active { get; set; } = true;
        public string Description { get; set; }

    }
}
