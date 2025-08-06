
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Data.Repositories;
using VisualSoft.Surveillance.Payment.Domain.Models;

public class TarifRepository : ITarifRepository
{
    private readonly IDbConnection _connection;
    private readonly IUserIdentificationModel _logedinUser;

    public TarifRepository(IDbConnection connection,IUserIdentificationModel logedinUser)
    {
        _connection = connection;
        _logedinUser = logedinUser;
    }

    public async Task<IEnumerable<TarifDataModel>?> GetAll()
    {
        
        var sql = @"SELECT * FROM Tarif;";
        return await _connection.QueryAsync<TarifDataModel>(sql);
    }

    public async Task<TarifDataModel?> GetById(Guid id)
    {
        var sql = @"SELECT * FROM Tarif WHERE id = @Id;";
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);

        return await _connection.QueryFirstOrDefaultAsync<TarifDataModel>(sql, parameters);
    }

    public async Task<TarifDataModel?> GetTarif(Guid vehicleTypeId, Guid tarifTypeId)
    {
        var sql = @"
        SELECT * 
        FROM   Tarif
        WHERE  vehicle_type_id = @VehicleTypeId
          AND  tarif_type_id   = @TarifTypeId
          AND  is_active       = true;";

        var parameters = new DynamicParameters();
        parameters.Add("VehicleTypeId", vehicleTypeId);
        parameters.Add("TarifTypeId", tarifTypeId);

        return await _connection.QueryFirstOrDefaultAsync<TarifDataModel>(
            sql,
            parameters
        );
    }


    public async Task<TarifDataModel?> Create(TarifDataModel tarif)
    {
        var sql = @"
            INSERT INTO Tarif (
                vehicle_type_id,
                tarif_type_id,
                is_active,
                description,
                created_date,
                updated_date,
                created_by,
                updated_by
            ) VALUES (
                @Vehicle_Type_Id,
                @Tarif_Type_Id,
                @Is_Active,
                @Description,
                @Created_Date,
                @Updated_Date,
                @Created_By,
                @Updated_By
            )";

        var parameters = new DynamicParameters();
        parameters.Add("@Vehicle_Type_Id", tarif.Vehicle_Type_Id);
        parameters.Add("@Tarif_Type_Id", tarif.Tarif_Type_Id);
        parameters.Add("@Is_Active", tarif.Is_Active);
        parameters.Add("@Description", tarif.Description);
        parameters.Add("@Created_Date", tarif.Created_Date);
        parameters.Add("@Updated_Date", tarif.Updated_Date);
        parameters.Add("@Created_By", tarif.Created_By);
        parameters.Add("@Updated_By", tarif.Updated_By);


        return await _connection.QueryFirstOrDefaultAsync<TarifDataModel>(sql, parameters);
    }

    public async Task<TarifDataModel?> Update(TarifDataModel tarif)
    {
        var sql= @"
            UPDATE Tarif SET
                vehicle_type_id = @Vehicle_Type_Id,
                tarif_type_id   = @Tarif_Type_Id,
                is_active       = @Is_Active,
                description     = @Description,
                updated_by      = @Updated_By,
                updated_date    = @Updated_Date
            WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("@Id", tarif.Id);
        parameters.Add("@Vehicle_Type_Id", tarif.Vehicle_Type_Id);
        parameters.Add("@Tarif_Type_Id", tarif.Tarif_Type_Id);
        parameters.Add("@Is_Active", tarif.Is_Active);
        parameters.Add("@Description", tarif.Description);
        parameters.Add("@Updated_By", tarif.Updated_By);
        parameters.Add("@Updated_Date", tarif.Updated_Date);


        return await _connection.QueryFirstOrDefaultAsync<TarifDataModel>(sql, parameters);
    }

    public async Task<bool> Delete(Guid id)
    {
        var sql = @"DELETE FROM Tarif WHERE id = @Id";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);

        var affectedRows = await _connection.ExecuteAsync(sql, parameters);
        return affectedRows > 0;
    }
}
