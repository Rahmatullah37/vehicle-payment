namespace VisualSoft.Surveillance.Payment.API.Models.Response
{
    public class PackageResponseModel
    {
        public Guid Id { get; set; }

        public string Package_Type { get; set; }
        public decimal Package_Cost { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
