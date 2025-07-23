namespace VehicleSurveillance.Payment.API.Models.Response
{
    public class TarifTypeResponseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
    }
}
