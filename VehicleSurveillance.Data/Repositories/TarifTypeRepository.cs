using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public class TarifTypeRepository:ITarifTypeRepository
    {
       
            private readonly IDbConnection _connection;
            private readonly IDbTransaction _transaction;

            public TarifTypeRepository(IDbConnection connection, IDbTransaction transaction)
            {
                _connection = connection;
                _transaction = transaction;
            }

            public List<TarifTypeDataModel> GetAll()
            {
                return _connection.Query<TarifTypeDataModel>("SELECT * FROM TarifType", transaction: _transaction).ToList();
            }

            public TarifTypeDataModel GetById(Guid id)
            {
                return _connection.QueryFirstOrDefault<TarifTypeDataModel>(
                    "SELECT * FROM TarifType WHERE id = @Id",
                    new { Id = id },
                    transaction: _transaction
                );
            }

            public void Create(TarifTypeDataModel fixedTarif)
            {
                _connection.Execute(
                    @"INSERT INTO TarifType (name,  is_active, created_date, updated_date, created_by, updated_by)
                  VALUES (@Name, @Is_Active, @Created_Date, @Updated_Date, @Created_By, @Updated_By)",
                    fixedTarif,
                    transaction: _transaction
                );
            }

            public void Update(TarifTypeDataModel fixedTarif)
            {
                _connection.Execute(
                    @"UPDATE TarifType SET
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
                    "DELETE FROM TarifType WHERE id = @Id",
                    new { Id = id },
                    transaction: _transaction
                );
            }


        // ✅ 1. Get the TarifTypeDataModel for 'Fixed'
        public TarifTypeDataModel GetFixedTarifType()
        {
            return _connection.QueryFirstOrDefault<TarifTypeDataModel>(
                "SELECT * FROM TarifType WHERE LOWER(name) = LOWER(@Name)",
                new { Name = "Fixed" },
                transaction: _transaction
            );
        }

        // Get the TarifTypeEnum from a tarifTypeId
        public TarifTypeEnum GetTarifType(Guid tarifTypeId)
        {
            var type = _connection.QueryFirstOrDefault<string>(
                "SELECT name FROM tariftype WHERE id = @Id",
                new { Id = tarifTypeId },
                transaction: _transaction
            );

            if (type == null)
                throw new Exception("Tarif type not found with given ID");

            return Enum.TryParse<TarifTypeEnum>(type, ignoreCase: true, out var result)
                ? result
                : throw new Exception("Unknown tarif type: " + type);
        }

        public TarifTypeDataModel GetHourlyTarifType()
        {
            return _connection.QueryFirstOrDefault<TarifTypeDataModel>(
                "SELECT * FROM TarifType WHERE name = @Name",
                new { Name = "Hourly" },
                transaction: _transaction
            );
        }


        //Calculate amount based on TarifType
        //public decimal GetAmountByTarif(Guid tarifId, TarifTypeEnum type, int totalHours)
        //{
        //    switch (type)
        //    {
        //        case TarifTypeEnum.Fixed:
        //            return _connection.QueryFirstOrDefault<decimal>(
        //                "SELECT amount FROM fixedtarif WHERE tarif_id = @TarifId AND is_active = true",
        //                new { TarifId = tarifId },
        //                transaction: _transaction
        //            );

        //        case TarifTypeEnum.Hourly:
        //            return _connection.QueryFirstOrDefault<decimal>(
        //                @"SELECT amount FROM hourlytarif 
        //              WHERE tarif_id = @TarifId 
        //                AND is_active = true 
        //                AND @Hours >= from_hour AND @Hours <= to_hour",
        //                new { TarifId = tarifId, Hours = totalHours },
        //                transaction: _transaction
        //            );

        //        case TarifTypeEnum.DistanceBased:
        //            throw new NotImplementedException("Distance-based tarif not implemented yet");

        //        default:
        //            throw new Exception("Invalid tarif type");
        //    }
        //}

        public decimal GetAmountByTarif(Guid tarifId, TarifTypeEnum type, int totalHours)
        {
            switch (type)
            {
                case TarifTypeEnum.Fixed:
                    return _connection.QueryFirstOrDefault<decimal>(
                        "SELECT amount FROM fixedtarif WHERE tarif_id = @TarifId AND is_active = true",
                        new { TarifId = tarifId },
                        transaction: _transaction
                    );

                case TarifTypeEnum.Hourly:
                    return GetAmountByHourlyHours(tarifId, totalHours); // 👈 Call helper method

                case TarifTypeEnum.DistanceBased:
                    throw new NotImplementedException("Distance-based tarif not implemented yet");

                default:
                    throw new Exception("Invalid tarif type");
            }
        }

                private decimal GetAmountByHourlyHours(Guid tarifId, int hours)
                {
                    return _connection.QueryFirstOrDefault<decimal>(
                        @"SELECT amount FROM hourlytarif 
                        WHERE tarif_id = @TarifId 
                        AND is_active = true 
                        AND @Hours BETWEEN from_hour AND to_hour",
                        new { TarifId = tarifId, Hours = hours },
                        transaction: _transaction
                    );
                }

    }
}

