
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class HourlyTarifRepository : IHourlyTarifRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _logedinUser;

        public HourlyTarifRepository(IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }

      
        public async Task<IEnumerable<HourlyTarifDataModel>> GetAll()
        {
          
            
            var sql = "SELECT * FROM hourlytarif;";
            return await _connection.QueryAsync<HourlyTarifDataModel>(sql);
        }

        public async Task<HourlyTarifDataModel?> GetById(Guid id)
        {
            var sql = "SELECT * FROM hourlytarif WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<HourlyTarifDataModel>(sql, parameters);
        }

        public async Task<HourlyTarifDataModel?> Create(HourlyTarifDataModel hourlyTarif)
        {
            var sql= @"
                INSERT INTO hourlytarif (
                    from_hour,
                    to_hour,
                    amount,
                    tarif_id,
                    is_active,
                    created_by,
                    created_date,
                    updated_by,
                    updated_date
                )
                VALUES (
                    @From_Hour,
                    @To_Hour,
                    @Amount,
                    @Tarif_Id,
                    @Is_Active,
                    @Created_By,
                    @Created_Date,
                    @Updated_By,
                    @Updated_Date
                )";

            var parameters = new DynamicParameters();
            parameters.Add("@From_Hour", hourlyTarif.From_Hour);
            parameters.Add("@To_Hour", hourlyTarif.To_Hour);
            parameters.Add("@Amount", hourlyTarif.Amount);
            parameters.Add("@Tarif_Id", hourlyTarif.Tarif_Id);
            parameters.Add("@Is_Active", hourlyTarif.Is_Active);
            parameters.Add("@Created_By", hourlyTarif.Created_By);
            parameters.Add("@Created_Date", hourlyTarif.Created_Date);
            parameters.Add("@Updated_By", hourlyTarif.Updated_By);
            parameters.Add("@Updated_Date", hourlyTarif.Updated_Date);


            return await _connection.QueryFirstOrDefaultAsync<HourlyTarifDataModel>(sql, parameters);
        }

        public async Task<HourlyTarifDataModel?> Update(HourlyTarifDataModel hourlyTarif)
        {
            var sql = @"
                UPDATE hourlytarif SET
                    from_hour    = @From_Hour,
                    to_hour      = @To_Hour,
                    amount       = @Amount,
                    tarif_id     = @Tarif_Id,
                    is_active    = @Is_Active,
                    updated_by   = @Updated_By,
                    updated_date = @Updated_Date
                WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", hourlyTarif.Id);
            parameters.Add("@From_Hour", hourlyTarif.From_Hour);
            parameters.Add("@To_Hour", hourlyTarif.To_Hour);
            parameters.Add("@Amount", hourlyTarif.Amount);
            parameters.Add("@Tarif_Id", hourlyTarif.Tarif_Id);
            parameters.Add("@Is_Active", hourlyTarif.Is_Active);
            parameters.Add("@Updated_By", hourlyTarif.Updated_By);
            parameters.Add("@Updated_Date", hourlyTarif.Updated_Date);


            return await _connection.QueryFirstOrDefaultAsync<HourlyTarifDataModel>(sql, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = @"DELETE FROM hourlytarif WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);

            return affectedRows > 0;
        }
    }
}
