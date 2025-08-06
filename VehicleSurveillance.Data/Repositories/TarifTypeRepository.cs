
using Dapper;
using System.Data;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class TarifTypeRepository : ITarifTypeRepository
    {

        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _logedinUser;

        public TarifTypeRepository(IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }

        public async Task<IEnumerable<TarifTypeDataModel>> GetAll()
        {
           
            var sql = @"SELECT * FROM TarifType;";
            return await _connection.QueryAsync<TarifTypeDataModel>(sql);
        }

        public async Task<TarifTypeDataModel?> GetById(Guid id)
        {
            var sql = @"SELECT * FROM TarifType WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<TarifTypeDataModel>(sql, parameters);
        }

        public async Task<TarifTypeDataModel?> Create(TarifTypeDataModel tarifType)
        {
            var sql = @"
                INSERT INTO TarifType (
                    name,
                    is_active,
                    created_date,
                    updated_date,
                    created_by,
                    updated_by
                ) VALUES (
                    @Name,
                    @Is_Active,
                    @Created_Date,
                    @Updated_Date,
                    @Created_By,
                    @Updated_By
                )";

            var parameters = new DynamicParameters();
            parameters.Add("Name", tarifType.Name);
            parameters.Add("Is_Active", tarifType.Is_Active);
            parameters.Add("Created_Date", tarifType.Created_Date);
            parameters.Add("Updated_Date", tarifType.Updated_Date);
            parameters.Add("Created_By", tarifType.Created_By);
            parameters.Add("Updated_By", tarifType.Updated_By);

            return await _connection.QueryFirstOrDefaultAsync<TarifTypeDataModel>(sql, parameters);
        }

        public async Task<TarifTypeDataModel?> Update(TarifTypeDataModel tarifType)
        {
            var sql= @"
                UPDATE TarifType
                SET
                    name         = @Name,
                    is_active    = @Is_Active,
                    updated_date = @Updated_Date,
                    updated_by   = @Updated_By
                WHERE id        = @Id";

            var parameters = new DynamicParameters();
            parameters.Add("Id", tarifType.Id);
            parameters.Add("Name", tarifType.Name);
            parameters.Add("Is_Active", tarifType.Is_Active);
            parameters.Add("Updated_Date", tarifType.Updated_Date);
            parameters.Add("Updated_By", tarifType.Updated_By);

            return await _connection.QueryFirstOrDefaultAsync<TarifTypeDataModel>(sql, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = @"DELETE FROM TarifType WHERE id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public async Task<TarifTypeEnum> GetTarifType(Guid tarifTypeId)
        {
            const string query = @"SELECT name FROM TarifType WHERE id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", tarifTypeId);

            string? name = await _connection.QueryFirstOrDefaultAsync<string>(query, parameters);

            if (name == null)
            {
                throw new Exception("Tarif type not found with given ID.");
            }

            return Enum.TryParse(name, true, out TarifTypeEnum result)
                ? result
                : throw new Exception($"Invalid tarif type: {name}");
        }
        public async Task<TarifTypeDataModel?> GetFixedTarifType()
        {
            const string query = @"SELECT * FROM TarifType WHERE LOWER(name) = LOWER(@Name)";
            var parameters = new DynamicParameters();
            parameters.Add("Name", "Fixed");

           
            return await _connection.QueryFirstOrDefaultAsync<TarifTypeDataModel>(query, parameters);
        }
       
        public async Task<TarifTypeDataModel?> GetHourlyTarifType()
        {
            const string query = @"SELECT * FROM TarifType WHERE name = @Name";
            var parameters = new DynamicParameters();
            parameters.Add("Name", "Hourly");

            return await _connection.QueryFirstOrDefaultAsync<TarifTypeDataModel>(query, parameters);
        }

        public async Task<decimal> GetAmountByTarif(Guid tarifId, TarifTypeEnum type, int totalHours)
        {
           
            return type switch
            {
                TarifTypeEnum.Fixed => await GetFixedTarifAmount(tarifId),
                TarifTypeEnum.Hourly => await GetHourlyTarifAmount(tarifId, totalHours),
                TarifTypeEnum.DistanceBased => throw new NotImplementedException("Distance-based tarif not implemented."),
                _ => throw new ArgumentException("Unknown tarif type."),
            };
        }

        private async Task<decimal> GetFixedTarifAmount(Guid tarifId)
        {
            const string query = @"SELECT amount FROM FixedTarif WHERE tarif_id = @TarifId AND is_active = true";
            var parameters = new DynamicParameters();
            parameters.Add("TarifId", tarifId);

            return await _connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
        }

        private async Task<decimal> GetHourlyTarifAmount(Guid tarifId, int hours)
        {
            const string query = @"
                SELECT amount
                FROM HourlyTarif
                WHERE
                    tarif_id = @TarifId
                    AND is_active = true
                    AND @Hours BETWEEN from_hour AND to_hour";

            var parameters = new DynamicParameters();
            parameters.Add("TarifId", tarifId);
            parameters.Add("Hours", hours);

            return await _connection.QueryFirstOrDefaultAsync<decimal>(query, parameters);
        }
       
       
    }
}
