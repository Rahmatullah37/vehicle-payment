using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public class TarifRepository:ITarifRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public TarifRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public List<TarifDataModel> GetAll()
        {
            return _connection.Query<TarifDataModel>("SELECT * FROM Tarif", transaction: _transaction).ToList();
        }

        public TarifDataModel GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<TarifDataModel>(
                "SELECT * FROM Tarif WHERE id = @Id",
                new { Id = id },
                transaction: _transaction
            );
        }

        public void Create(TarifDataModel tarif)
        {
            _connection.Execute(
                @"INSERT INTO Tarif (vehicle_type_id, tarif_type_id, is_active, description, created_date, updated_date, created_by, updated_by)
                  VALUES (@Vehicle_Type_Id, @Tarif_Type_Id, @Is_Active, @Description, @Created_Date, @Updated_Date, @Created_By, @Updated_By)",
                tarif,
                transaction: _transaction
            );
        }

        public void Update(TarifDataModel tarif)
        {
            _connection.Execute(
                @"UPDATE Tarif SET
                  vehicle_type_id = @Vehicle_Type_Id,
                  tarif_type_id = @Tarif_Type_Id,
                  is_active = @Is_Active,
                  description = @Description,
                  updated_by = @Updated_By,
                  updated_date = @Updated_Date
                  WHERE id = @Id",
                tarif,
                transaction: _transaction
            );
        }

        public void Delete(Guid id)
        {
            _connection.Execute(
                "DELETE FROM Tarif WHERE id = @Id",
                new { Id = id },
                transaction: _transaction
            );
        }
        public TarifDataModel GetTarif(Guid vehicleTypeId, Guid tarifTypeId)
        {
            return _connection.QueryFirstOrDefault<TarifDataModel>(
                @"SELECT * FROM Tarif
                WHERE vehicle_type_id = @VehicleTypeId AND tarif_type_id = @TarifTypeId AND is_active = true",
                new { VehicleTypeId = vehicleTypeId, TarifTypeId = tarifTypeId },
                transaction: _transaction
            );
        }

    }
}
