using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Models
{
    [Table("DistanceTarif")]
    public class DistanceTarifDataModel:BaseDto
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("EntryBooth")]
        public Guid EntryBoothId { get; set; }

        [ForeignKey("ExitBooth")]
        public Guid ExitBoothId { get; set; }

        public decimal RatePerKm { get; set; }

        public decimal Distance { get; set; }

        [ForeignKey("Tarif")]
        public Guid TarifId { get; set; }

      
    }

}
