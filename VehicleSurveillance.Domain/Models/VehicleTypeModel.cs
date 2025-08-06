using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class VehicleTypeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Is_Active { get; set; } = true;
        public DateTime Created_Date { get; set; }
        public DateTime Updated_Date { get; set; }
        public string Created_By { get; set; }
        public string Updated_By { get; set; }
    }
}
