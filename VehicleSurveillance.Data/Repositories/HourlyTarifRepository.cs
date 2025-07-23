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
    public class HourlyTarifRepository:IHourlyTarifRepository
    {
           private readonly IDbConnection _connection;
            private readonly IDbTransaction _transaction;
            public HourlyTarifRepository(IDbConnection connection, IDbTransaction transaction)
            {
                _connection = connection;
                _transaction = transaction;
            }
            public List<HourlyTarifDataModel> GetAll()
            {
                return _connection.Query<HourlyTarifDataModel>("Select * from hourlytarif", transaction: _transaction).ToList();
            }
            public HourlyTarifDataModel GetById(Guid id)
            {
                return _connection.QueryFirstOrDefault<HourlyTarifDataModel>("SELECT * FROM hourlytarif WHERE id = @Id", new { Id = id }, transaction: _transaction);
            }
            public void Create(HourlyTarifDataModel hourlyTarif)
            {
                _connection.Execute(
                    @"INSERT INTO hourlytarif (from_hour, to_hour,amount,tarif_id, is_active, created_by, created_date, updated_by, updated_date) 
                  VALUES (@From_Hour, @To_hour,@Amount,@Tarif_Id, @Is_Active, @Created_By, @Created_Date, @Updated_Date, @Updated_Date)",
                    hourlyTarif,
                    transaction: _transaction
                );
            }
            public void Update(HourlyTarifDataModel hourlyTarif)
            {

                        _connection.Execute(
                    @"UPDATE hourlytarif SET
                from_hour = @From_Hour,
                to_hour = @To_hour,
                amount=@Amount,
                tarif_id=@Tarif_Id,
                is_active = @Is_Active,
                updated_by = @Updated_By,
                updated_date = @Updated_Date
                WHERE id = @Id",
                  hourlyTarif,
                  transaction: _transaction
              );
            }
            public void Delete(Guid id)
            {
                _connection.Execute("DELETE FROM hourlytarif WHERE id = @Id", new { Id = id }, transaction: _transaction);
            }


        }
    
}
