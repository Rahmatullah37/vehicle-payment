using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class TollBoothRepository:ITollBoothRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _userlogin;
        public  TollBoothRepository(IDbConnection connection,IUserIdentificationModel userlogin)
        {
            _connection = connection;
            _userlogin = userlogin;
        }

        public async Task<IEnumerable<TollBoothDataModel>>GetAll()
        {
            var sql = "Select * from tollbooth;";
            return await _connection.QueryAsync < TollBoothDataModel > (sql);

        }
        public async Task<TollBoothDataModel> GetById(Guid id)
        {
            var sql = @"SELECT * FROM tollbooth  WHERE id=@ID;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id",id);
            return await _connection.QueryFirstOrDefaultAsync<TollBoothDataModel>(sql, parameters);
        }
        public async Task<TollBoothDataModel> Create(TollBoothDataModel tollBooth)
        {
            var sql = @"insert into tollbooth(
                id,
                name,
                location,
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
            parameters.Add("@Id", tollBooth.Id);
            parameters.Add("@Name", tollBooth.Name);
            parameters.Add("@Location", tollBooth.Location);
            parameters.Add("@Is_Active", tollBooth.IsActive);
            parameters.Add("@Created_Date", tollBooth.CreatedDate);
            parameters.Add("@Created_By", tollBooth.CreatedBy);
            parameters.Add("@Update_By", tollBooth.UpdatedBy);
            parameters.Add("@Update_Date", tollBooth.UpdatedDate);
            return await _connection.QueryFirstOrDefaultAsync<TollBoothDataModel>(sql, parameters);

        }
        public async Task<TollBoothDataModel> Update(TollBoothDataModel tollBooth)
        {
            var sql = @"UPDATE tollbooth SET(
                
                name = @Name,
                location     = @Location,
                is_active    = @Is_Active,
                updated_date = @Updated_Date,
                updated_by   = @Updated_By
                WHERE id = @Id
                RETURNING *;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", tollBooth.Id);
            parameters.Add("@Name", tollBooth.Name);
            parameters.Add("@Location", tollBooth.Location);
            parameters.Add("@Is_Active", tollBooth.IsActive);
            parameters.Add("@Update_By", tollBooth.UpdatedBy);
            parameters.Add("@Update_Date", tollBooth.UpdatedDate);
            return await _connection.QueryFirstOrDefaultAsync<TollBoothDataModel>(sql, parameters);

        }
        public async Task<bool> Delete(Guid id)
        {
            var sql = "DELETE FROM tollbooth WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);

            return affectedRows > 0;
        }
        

        public async Task<(Guid? EnterBoothId, Guid? ExitBoothId)> GetBoothIds(string enterBooth, string exitBooth)
        {
            var sql = "SELECT id, name FROM tollbooth WHERE TRIM(LOWER(name)) IN (@EnterBooth, @ExitBooth)";

            var booths = await _connection.QueryAsync<(Guid Id, string Name)>(sql, new
            {
                EnterBooth = enterBooth.Trim().ToLower(),
                ExitBooth = exitBooth.Trim().ToLower()
            });

            var enterBoothId = booths.FirstOrDefault(b => b.Name.Trim().ToLower() == enterBooth.Trim().ToLower()).Id;
            var exitBoothId = booths.FirstOrDefault(b => b.Name.Trim().ToLower() == exitBooth.Trim().ToLower()).Id;//extract the booth IDs for each name, and return them

            return (enterBoothId == Guid.Empty ? null : enterBoothId,
                    exitBoothId == Guid.Empty ? null : exitBoothId);
        }

        public async Task<string> GetBoothNameById(Guid boothId)
        {
            var sql = "SELECT name FROM tollbooth WHERE Id = @Id";
            return await _connection.QueryFirstOrDefaultAsync<string>(sql, new { Id = boothId });
        }

    }
}
