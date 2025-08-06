using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class DistanceTarifDomainModel
    {
        public Guid Id { get; set; }
        public Guid EntryBoothId { get; set; }
        public Guid ExitBoothId { get; set; }
        public decimal RatePerKm { get; set; }
        public double Distance { get; set; }
        public Guid TarifId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
