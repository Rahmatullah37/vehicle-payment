using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Data.Models
{
    [Table("DistanceTarif")]
    public class DistanceTarifDataModel
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("EntryBooth")]
        public Guid EntryBoothId { get; set; }

        [ForeignKey("ExitBooth")]
        public Guid ExitBoothId { get; set; }

        public decimal RatePerKm { get; set; }

        public double Distance { get; set; }

        [ForeignKey("Tarif")]
        public Guid TarifId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        [Required]
        public string UpdatedBy { get; set; }
    }

}
