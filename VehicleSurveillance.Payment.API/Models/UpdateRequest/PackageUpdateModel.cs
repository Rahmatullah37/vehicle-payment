namespace VisualSoft.Surveillance.Payment.API.Models.UpdateRequest
{
    public class PackageUpdateModel
    {

        public string Package_Type { get; set; }
        public decimal Package_Cost { get; set; }
        public bool Is_Active { get; set; } = true;
    }
}
