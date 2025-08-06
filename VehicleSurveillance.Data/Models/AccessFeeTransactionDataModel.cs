using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Data.Models
{
    public class AccessFeeTransactionDataModel
    {
        public Guid Id { get; set; }
        public Guid Vehicle_Id { get; set; } 
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; } 
        public string Payment_Mode { get; set; }  // this filed should be cash, Package, Tag, VehicleAccount
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; } 
        public string Created_By { get; set; } 
        public string Updated_By { get; set; }
        public Guid Category_Id { get; set; }

     
        public bool IsPackageTransaction { get; set; }
        public Guid? PackageId { get; set; }
    }
}
