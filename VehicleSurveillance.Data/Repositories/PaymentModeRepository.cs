
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class PaymentModeRepository : IPaymentModeRepository
    {
        private readonly IDbConnection _connection;

        private readonly IUserIdentificationModel _logedinUser;


        public PaymentModeRepository(IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }


        public async Task<IEnumerable<PaymentModeDataModel>> GetAll()
        {
           
            var sql = @"SELECT * FROM PaymentModes;";
            return await _connection.QueryAsync<PaymentModeDataModel>(sql);
        }

        public async Task<PaymentModeDataModel?> GetById(Guid id)
        {
            var sql = @"SELECT * FROM PaymentModes WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<PaymentModeDataModel>(sql, parameters);
        }

        public async Task<PaymentModeDataModel?> GetPaymentByName(string name)
        {
            var sql = @"
                        SELECT * 
                        FROM   PaymentModes
                        WHERE  name = @Name;";

            var parameters = new DynamicParameters();
            parameters.Add("Name", name);
            return await _connection.QueryFirstOrDefaultAsync<PaymentModeDataModel>(
                sql,
                parameters
            );
        }


        public async Task<PaymentModeDataModel?> Create(PaymentModeDataModel mode)
        {
            var sql = @"
                INSERT INTO PaymentModes (
                    name
                ) VALUES (
                    @Name
                )";
            var parameters = new DynamicParameters();
            parameters.Add("@Name", mode.Name);

            return await _connection.QueryFirstOrDefaultAsync<PaymentModeDataModel>(sql, parameters);
        }

        public async Task<PaymentModeDataModel?> Update(PaymentModeDataModel mode)
        {
            var sql = @"
                UPDATE PaymentModes SET
                    name = @Name
                WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", mode.Id);
            parameters.Add("@Name", mode.Name);


            return await _connection.QueryFirstOrDefaultAsync<PaymentModeDataModel>(sql, parameters);
            
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = @"DELETE FROM PaymentModes WHERE id = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }
    }
}
