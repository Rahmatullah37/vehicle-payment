using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Data.Models
{
    public class VehicleSummaryDataModel
    {
        public string VehicleCategory { get; set; }
        public string PaymentMode { get; set; }
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
}
