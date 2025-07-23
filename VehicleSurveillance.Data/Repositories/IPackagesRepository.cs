using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Data.Repositories
{
     public interface IPackagesRepository
     {
        List<PackageDataModel> GetAll();
        PackageDataModel GetById(Guid id);
        void Create(PackageDataModel package);
        void Delete(Guid id);
        void Update(PackageDataModel package);
       

    }

}
