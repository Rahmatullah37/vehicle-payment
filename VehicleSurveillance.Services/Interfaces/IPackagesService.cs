using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IPackagesService
    {
        List<PackageModel> GetAllPackages();
        PackageModel GetPackageById(Guid id);
        void AddPackage(PackageModel package);
        void UpdatePackage(PackageModel package);
        void DeletePackage(Guid id);

    }
}
