using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Models
{
    public class TollBoothDataModel:BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; } 
       
    }
}
