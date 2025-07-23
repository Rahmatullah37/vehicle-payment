using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public class PaymentModeRepository : IPaymentModeRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public PaymentModeRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public List<PaymentModeDataModel> GetAll()
        {
            return _connection.Query<PaymentModeDataModel>("SELECT * FROM PaymentModes", transaction: _transaction).ToList();
        }

        public PaymentModeDataModel GetById(Guid id)
        {
            return _connection.QueryFirstOrDefault<PaymentModeDataModel>(
                "SELECT * FROM PaymentModes WHERE id = @Id", new { Id = id }, transaction: _transaction);
        }

        public void Create(PaymentModeDataModel mode)
        {
            _connection.Execute(
                "INSERT INTO PaymentModes ( name) VALUES ( @Name)",
                mode,
                transaction: _transaction);
        }

        public void Update(PaymentModeDataModel mode)
        {
            _connection.Execute(
                "UPDATE PaymentModes SET name = @Name WHERE id = @Id",
                mode,
                transaction: _transaction);
        }

        public void Delete(Guid id)
        {
            _connection.Execute("DELETE FROM PaymentModes WHERE id = @Id", new { Id = id }, transaction: _transaction);
        }

        public PaymentModeDataModel GetPaymentByName(string name)
        {
            return _connection.QueryFirstOrDefault<PaymentModeDataModel>(
                "SELECT * FROM PaymentModes WHERE name = @Name",
                new { Name = name },
                transaction: _transaction
            );
        }
    }
}
