using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public class PackagesRepository:IPackagesRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;
        public PackagesRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }
        public List<PackageDataModel> GetAll()
        {
            return _connection.Query<PackageDataModel>("Select * from Packages", transaction: _transaction).ToList();
        }
        public PackageDataModel GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<PackageDataModel>("SELECT * FROM Packages WHERE id = @Id", new { Id = id }, transaction: _transaction);
        }
        public void Create(PackageDataModel package)
        {
            _connection.Execute(
                @"INSERT INTO Packages (package_type, package_cost, is_active, created_by, created_date, updated_by, updated_date) 
                  VALUES (@Package_Type, @Package_Cost, @Is_Active, @Created_By, @Created_Date, @Updated_Date, @Updated_Date)",
                package,
                transaction: _transaction
            );
        }
        public void Update(PackageDataModel package)
        {
            
            _connection.Execute(
        @"UPDATE Packages SET
        package_type = @Package_Type,
        package_cost = @Package_Cost,
        is_active = @Is_Active,
        updated_by = @Updated_By,
        updated_date = @Updated_Date
        WHERE id = @Id",
      package,
      transaction: _transaction
  );
        }
        public void Delete(Guid id)
        {
            _connection.Execute("DELETE FROM Packages WHERE id = @Id", new { Id = id }, transaction: _transaction);
        }
    }
}
