using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Models
{
    public class PackageDataModel:BaseDto
    {
        
            public Guid Id { get; set; } 
            public string Package_Type { get; set; } 
            public decimal Package_Cost { get; set; }


    }
    
}
