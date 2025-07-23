using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VehicleSurveillance.Data.Models;


namespace VehicleSurveillance.Data.Repositories
{
    public class FixedTarifRepository:IFixedTarifRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public FixedTarifRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public List<FixedTarifDataModel> GetAll()
        {
            return _connection.Query<FixedTarifDataModel>("SELECT * FROM FixedTarif", transaction: _transaction).ToList();
        }

        public FixedTarifDataModel GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<FixedTarifDataModel>(
                "SELECT * FROM FixedTarif WHERE id = @Id",
                new { Id = id },
                transaction: _transaction
            );
        }

        public void Create(FixedTarifDataModel fixedTarif)
        {
            _connection.Execute(
                @"INSERT INTO FixedTarif (amount, tarif_id, is_active, created_date, updated_date, created_by, updated_by)
                  VALUES (@Amount, @Tarif_Id, @Is_Active, @Created_Date, @Updated_Date, @Created_By, @Updated_By)",
                fixedTarif,
                transaction: _transaction
            );
        }

        public void Update(FixedTarifDataModel fixedTarif)
        {
            _connection.Execute(
                @"UPDATE FixedTarif SET
                  amount = @Amount,
                  tarif_id = @Tarif_Id,
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
                "DELETE FROM FixedTarif WHERE id = @Id",
                new { Id = id },
                transaction: _transaction
            );
        }
        public FixedTarifDataModel GetByTarifId(Guid tarifId)
        {
            return _connection.QueryFirstOrDefault<FixedTarifDataModel>(
                "SELECT * FROM FixedTarif WHERE tarif_id = @TarifId AND is_active = true",
                new { TarifId = tarifId },
                transaction: _transaction
            );
        }

    }
}
