using Dapper;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class DistanceTarifRepository: IDistanceTarifRepository
    {
      
            private readonly IDbConnection _connection;
            private readonly IUserIdentificationModel _userlogin;
            public DistanceTarifRepository(IDbConnection connection, IUserIdentificationModel userlogin)
            {
                _connection = connection;
                _userlogin = userlogin;
            }

            public async Task<IEnumerable<DistanceTarifDataModel>> GetAll()
            {
                var sql = "Select * from distancetarif;";
                return await _connection.QueryAsync<DistanceTarifDataModel>(sql);

            }
            public async Task<DistanceTarifDataModel> GetById(Guid id)
            {
                var sql = @"SELECT * FROM distancetarif  WHERE id=@ID;";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                return await _connection.QueryFirstOrDefaultAsync<DistanceTarifDataModel>(sql, parameters);
            }
            public async Task<DistanceTarifDataModel> Create(DistanceTarifDataModel distanceTarif)
            {
                var sql = @"insert into distancetarif(
                id,
                entrybooth,
                exitbooth,
                rateperkm,
                distance,
                tarifid,
                is_active,
                created_date,
                create_by,
                update_date,
                update_by)
                VAlUES(
                @Id
                @Name,
                @Location,
                @Is_Active,
                Created_Date,
                Create_By,
                Update_By,
                Update_Date)
                RETURNING *;";


                var parameters = new DynamicParameters();
                parameters.Add("@Id", distanceTarif.Id);
                parameters.Add("@EntryBooth", distanceTarif.EntryBoothId);
                parameters.Add("@ExitBooth", distanceTarif.ExitBoothId);
                parameters.Add("@RatePerKm", distanceTarif.RatePerKm);
                parameters.Add("@Distance", distanceTarif.Distance);
                parameters.Add("@TarifId", distanceTarif.TarifId);
                parameters.Add("@Is_Active", distanceTarif.IsActive);
                parameters.Add("@Created_Date", distanceTarif.CreatedDate);
                parameters.Add("@Created_By", distanceTarif.CreatedBy);
                parameters.Add("@Update_By", distanceTarif.UpdatedBy);
                parameters.Add("@Update_Date", distanceTarif.UpdatedDate);
                return await _connection.QueryFirstOrDefaultAsync<DistanceTarifDataModel>(sql, parameters);

            }
            public async Task<DistanceTarifDataModel> Update(DistanceTarifDataModel distanceTarif)
            {
                var sql = @"UPDATE distancetarif SET(
                
                entrybooth = @EntryBooth,
                exitbooth     = @ExitBooth,
                rateperkm = @RatePerKm,
                distance     = @Distance,
                tarifid     = @TarifId,
                is_active    = @Is_Active,
                updated_date = @Updated_Date,
                updated_by   = @Updated_By
                WHERE id = @Id
                RETURNING *;";
                    var parameters = new DynamicParameters();
                parameters.Add("@Id", distanceTarif.Id);
                parameters.Add("@EntryBooth", distanceTarif.EntryBoothId);
                parameters.Add("@ExitBooth", distanceTarif.ExitBoothId);
                parameters.Add("@RatePerKm", distanceTarif.RatePerKm);
                parameters.Add("@Distance", distanceTarif.Distance);
                parameters.Add("@TarifId", distanceTarif.TarifId);
                parameters.Add("@Is_Active", distanceTarif.IsActive);
                parameters.Add("@Update_By", distanceTarif.UpdatedBy);
                parameters.Add("@Update_Date", distanceTarif.UpdatedDate);
            return await _connection.QueryFirstOrDefaultAsync<DistanceTarifDataModel>(sql, parameters);

            }
            public async Task<bool> Delete(Guid id)
            {
                var sql = "DELETE FROM distancetarif WHERE id = @Id;";
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);

                var affectedRows = await _connection.ExecuteAsync(sql, parameters);

                return affectedRows > 0;
            }
        public async Task<List<DistanceTarifDataModel>> GetSegmentsForTariffAsync(Guid tarifId)
        {
            const string sql = @"SELECT * FROM distancetarif WHERE tarif_id = @TarifId ORDER BY id;";
            var result = await _connection.QueryAsync<DistanceTarifDataModel>(sql, new { TarifId = tarifId });
            return result.ToList();
        }

    }


}
