using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleSurveillance.Domain.Models
{
    public class PackageModel
    {
        
            public Guid Id { get; set; } 

            public string Package_Type { get; set; } 
            public decimal Package_Cost { get; set; } 
            public bool Is_Active { get; set; } 
            public DateTime Created_Date { get; set; } 

            public string Created_By { get; set; } 

            public DateTime Updated_Date { get; set; } 
            public string Updated_By { get; set; } 
        }
    
}
