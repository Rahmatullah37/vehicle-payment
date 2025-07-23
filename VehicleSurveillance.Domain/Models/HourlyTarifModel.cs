using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Domain.Models
{
    public class HourlyTarifModel
    {
        public Guid Id { get; set; }
        public int FromHour { get; set; }
        public int ToHour { get; set; }
        public decimal Amount { get; set; }
        public Guid Tarif_Id { get; set; }
        public bool Is_Active { get; set; } 
        public DateTime Created_Date { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
        public DateTime Updated_Date { get; set; }
    }

}
