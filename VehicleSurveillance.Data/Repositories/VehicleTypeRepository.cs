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
        public class VehicleTypeRepository : IVehicleTypeRepository
        {

            private readonly IDbConnection _connection;
            private readonly IDbTransaction _transaction;

            public VehicleTypeRepository(IDbConnection connection, IDbTransaction transaction)
            {
                _connection = connection;
                _transaction = transaction;
            }

            public List<VehicleTypeDataModel> GetAll()
            {
                return _connection.Query<VehicleTypeDataModel>("SELECT * FROM VehicleType", transaction: _transaction).ToList();
            }

            public VehicleTypeDataModel GetById(Guid id)
            {
                return _connection.QueryFirstOrDefault<VehicleTypeDataModel>(
                    "SELECT * FROM VehicleType WHERE id = @Id",
                    new { Id = id },
                    transaction: _transaction
                );
            }

            public void Create(VehicleTypeDataModel fixedTarif)
            {
                _connection.Execute(
                    @"INSERT INTO VehicleType (name,  is_active, created_date, updated_date, created_by, updated_by)
                      VALUES (@Name, @Is_Active, @Created_Date, @Updated_Date, @Created_By, @Updated_By)",
                    fixedTarif,
                    transaction: _transaction
                );
            }

            public void Update(VehicleTypeDataModel fixedTarif)
            {
                _connection.Execute(
                    @"UPDATE VehicleType SET
                      name = @Name,
                
                      is_active = @Is_Active,
                      updated_date = @Updated_Date,
                      updated_by = @Updated_By
                      WHERE id = @Id",
                    fixedTarif,
                    transaction: _transaction
                );
            }

            public void Delete(Guid id)
            {
                _connection.Execute(
                    "DELETE FROM VehicleType WHERE id = @Id",
                    new { Id = id },
                    transaction: _transaction
                );
            }
            public VehicleTypeDataModel GetByName(string name)
            {
                return _connection.QueryFirstOrDefault<VehicleTypeDataModel>(
                    "SELECT * FROM VehicleType WHERE name = @Name AND is_active = true",
                    new { Name = name },
                    transaction: _transaction
                );
            }

    }
}
