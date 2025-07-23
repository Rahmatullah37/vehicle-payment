using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Domain.Models
{
    public class AccessFeeTransactionModel
    {
        public Guid Id { get; set; }
        public Guid Vehicle_Id { get; set; }
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; } 
        public string Payment_Mode { get; set; }  
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; } 
        public string Created_By { get; set; } 
        public string Updated_By { get; set; }
        public Guid category_id { get; set; }
        
    }
}
