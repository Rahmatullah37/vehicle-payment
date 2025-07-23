namespace VehicleSurveillance.Payment.API.Models.Response
{
    public class TarifResponseModel
    {
        public Guid Id { get; set; }
        public Guid Vehicle_Type_Id { get; set; }
        public Guid Tarif_Type_Id { get; set; }
        public bool Is_Active { get; set; } = true;
        public string Description { get; set; }
        public DateTime Created_Date { get; set; }
    }
}
