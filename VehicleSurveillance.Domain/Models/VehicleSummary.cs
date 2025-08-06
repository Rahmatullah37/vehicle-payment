using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class VehicleSummary
    {
    
        public string VehicleCategory { get; set; }
        public string PaymentMode { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
