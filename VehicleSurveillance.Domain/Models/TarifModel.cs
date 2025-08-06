using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class TarifModel
    {
        public Guid Id { get; set; }
        public Guid Vehicle_Type_Id { get; set; }
        public Guid Tarif_Type_Id { get; set; }
        public bool Is_Active { get; set; } = true;
        public string Description { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
      // NEW: Package-related properties
        public bool IsPackageTransaction { get; set; }
        public string Message { get; set; }
    }
}
