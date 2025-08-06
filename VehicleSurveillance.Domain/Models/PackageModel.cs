using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class PackageModel
    {
        
            public Guid Id { get; set; } 

            public string Package_Type { get; set; } 
            public decimal Package_Cost { get; set; } 
            public bool IsActive { get; set; } 
            public DateTime CreatedDate { get; set; } 

            public string CreatedBy { get; set; } 

            public DateTime UpdatedDate { get; set; } 
            public string UpdatedBy { get; set; } 
        }
    
}
