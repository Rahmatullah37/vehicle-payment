using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Domain.Models
{
    public class TransactionReportResponse
    {
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }

        public string? FilteredByVehicleCategory { get; set; }
        public string? FilteredByPaymentMode { get; set; }
        public List<VehicleSummary> GroupedSummaries { get; set; } = new();
    }
}
