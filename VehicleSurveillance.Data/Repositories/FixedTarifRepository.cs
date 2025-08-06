
using Dapper;
using System.Data;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class FixedTarifRepository : IFixedTarifRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _logedinUser;

        public FixedTarifRepository(
           IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }

       


        public async Task<IEnumerable<FixedTarifDataModel>> GetAll()
        {
            var sql = "SELECT * FROM FixedTarif;";
            return await _connection.QueryAsync<FixedTarifDataModel>(sql);
        }

        public async Task<FixedTarifDataModel?> GetById(Guid id)
        {
            var sql = "SELECT * FROM FixedTarif WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<FixedTarifDataModel>(sql, parameters);
        }

        public async Task<FixedTarifDataModel?> GetByTarifId(Guid tarifId)
        {
            var sql = "SELECT * FROM FixedTarif WHERE tarif_id = @TarifId AND is_active = TRUE;";
            var parameters = new DynamicParameters();
            parameters.Add("@TarifId", tarifId);

            return await _connection.QueryFirstOrDefaultAsync<FixedTarifDataModel>(sql, parameters);
        }

        public async Task<FixedTarifDataModel?> Create(FixedTarifDataModel fixedTarif)
        {
            var sql = @"
                INSERT INTO FixedTarif (
                    id,
                    amount,
                    tarif_id,
                    is_active,
                    created_date,
                    updated_date,
                    created_by,
                    updated_by
                ) VALUES (
                    @Id,
                    @Amount,
                    @Tarif_Id,
                    @Is_Active,
                    @Created_Date,
                    @Updated_Date,
                    @Created_By,
                    @Updated_By
                )
                RETURNING *;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", fixedTarif.Id);
            parameters.Add("@Amount", fixedTarif.Amount);
            parameters.Add("@Tarif_Id", fixedTarif.Tarif_Id);
            parameters.Add("@Is_Active", fixedTarif.IsActive);
            parameters.Add("@Created_Date", fixedTarif.CreatedDate, DbType.DateTime);
            parameters.Add("@Updated_Date", fixedTarif.UpdatedDate, DbType.DateTime);
            // parameters.Add("@Created_By", fixedTarif.Created_By);
            parameters.Add("@Created_By", "system");
           // parameters.Add("@Updated_By", fixedTarif.Updated_By);
            parameters.Add("@Updated_By", "system");


            return await _connection.QueryFirstOrDefaultAsync<FixedTarifDataModel>(sql, parameters);
        }

        public async Task<FixedTarifDataModel?> Update(FixedTarifDataModel fixedTarif)
        {
            var sql = @"
                UPDATE FixedTarif SET
                    amount       = @Amount,
                    tarif_id     = @Tarif_Id,
                    is_active    = @Is_Active,
                    updated_date = @Updated_Date,
                    updated_by   = @Updated_By
                WHERE id = @Id
                RETURNING *;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", fixedTarif.Id);
            parameters.Add("@Amount", fixedTarif.Amount);
            parameters.Add("@Tarif_Id", fixedTarif.Tarif_Id);
            parameters.Add("@Is_Active", fixedTarif.IsActive);
            parameters.Add("@Updated_Date", fixedTarif.UpdatedDate);
            parameters.Add("@Updated_By", fixedTarif.UpdatedBy);


            return await _connection.QueryFirstOrDefaultAsync<FixedTarifDataModel>(sql, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = "DELETE FROM FixedTarif WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
         
            return affectedRows > 0;
        }
    }
}
