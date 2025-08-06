using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Models
{
    public class FixedTarifDataModel:BaseDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Guid Tarif_Id { get; set; }
       
       
    }
}
