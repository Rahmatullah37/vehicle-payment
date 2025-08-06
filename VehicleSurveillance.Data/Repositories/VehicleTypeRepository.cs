
using Dapper;
using System.Data;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;
using Microsoft.Extensions.Logging;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {

        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _logedinUser;

        public VehicleTypeRepository(
            IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _logedinUser = logedinUser;
        }

       

        public async Task<IEnumerable<VehicleTypeDataModel>> GetAll()
        {
            var sql = "SELECT * FROM VehicleType;";
            return await _connection.QueryAsync<VehicleTypeDataModel>(sql);
        }

        public async Task<VehicleTypeDataModel?> GetById(Guid id)
        {
            var sql = "SELECT * FROM VehicleType WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<VehicleTypeDataModel>(sql, parameters);
        }

        public async Task<VehicleTypeDataModel?> GetByName(string name)
        {
            var sql = "SELECT * FROM VehicleType WHERE name = @Name AND is_active = TRUE;";
            var parameters = new DynamicParameters();
            parameters.Add("@Name", name);

            return await _connection.QueryFirstOrDefaultAsync<VehicleTypeDataModel>(sql, parameters);
        }

        public async Task<VehicleTypeDataModel?> Create(VehicleTypeDataModel vehicleType)
        {
            var sql = @"
                INSERT INTO VehicleType (
                    id,
                    name,
                    is_active,
                    created_date,
                    updated_date,
                    created_by,
                    updated_by
                ) VALUES (
                    @Id,
                    @Name,
                    @Is_Active,
                    @Created_Date,
                    @Updated_Date,
                    @Created_By,
                    @Updated_By
                )
                RETURNING *;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", vehicleType.Id);
            parameters.Add("@Name", vehicleType.Name);
            parameters.Add("@Is_Active", vehicleType.Is_Active);
            parameters.Add("@Created_Date", vehicleType.Created_Date);
            parameters.Add("@Updated_Date", vehicleType.Updated_Date);
            parameters.Add("@Created_By", vehicleType.Created_By);
            parameters.Add("@Updated_By", vehicleType.Updated_By);


            return await _connection.QueryFirstOrDefaultAsync<VehicleTypeDataModel>(sql, parameters);
        }

        public async Task<VehicleTypeDataModel?> Update(VehicleTypeDataModel vehicleType)
        {
            var sql = @"
                UPDATE VehicleType SET
                    name         = @Name,
                    is_active    = @Is_Active,
                    updated_date = @Updated_Date,
                    updated_by   = @Updated_By
                WHERE id = @Id
                RETURNING *;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", vehicleType.Id);
            parameters.Add("@Name", vehicleType.Name);
            parameters.Add("@Is_Active", vehicleType.Is_Active);
            parameters.Add("@Updated_Date", vehicleType.Updated_Date);
            parameters.Add("@Updated_By", vehicleType.Updated_By);


            return await _connection.QueryFirstOrDefaultAsync<VehicleTypeDataModel>(sql, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = "DELETE FROM VehicleType WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var affectedRows = await _connection.ExecuteAsync(sql, parameters);
          
            return affectedRows > 0;
        }
    }
}
