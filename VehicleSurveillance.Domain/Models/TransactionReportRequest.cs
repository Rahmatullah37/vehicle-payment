using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class TransactionReportRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? VehicleCategory { get; set; }
        public string? PaymentMode { get; set; }
    }
}
