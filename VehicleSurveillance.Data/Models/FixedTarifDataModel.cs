using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Data.Models
{
    public class FixedTarifDataModel
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public Guid Tarif_Id { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; } 
       
    }
}
