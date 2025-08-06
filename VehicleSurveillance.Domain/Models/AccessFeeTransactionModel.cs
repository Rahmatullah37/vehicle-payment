

namespace VisualSoft.Surveillance.Payment.Domain.Models
{
    public class AccessFeeTransactionModel:BaseDto
    {
        public Guid Id { get; set; }
        public decimal AmountCharged { get; set; }
        public string Vehicle_Category { get; set; } 
        public string Payment_Mode { get; set; }          
        public Guid category_id { get; set; }
        // NEW: Package-related properties

        public Guid Vehicle_Id { get; set; }
        public bool IsPackageTransaction { get; set; }
        public Guid? PackageId { get; set; }

        // Navigation properties
        public VehicleTypeModel Vehicle { get; set; }
        public PackageModel Package { get; set; }

    }
}
