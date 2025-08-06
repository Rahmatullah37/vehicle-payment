
namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class TollBoothModel:BaseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; } 
       
    }
}
