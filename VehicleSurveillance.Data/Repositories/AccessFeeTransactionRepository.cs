
using Dapper;
using Microsoft.Extensions.Logging;
using System.Data;
using VisualSoft.Surveillance.Payment.Data.Infrastructure;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.Domain.Models;

namespace VisualSoft.Surveillance.Payment.Data.Repositories
{
    public class AccessFeeTransactionRepository : IAccessFeeTransactionRepository
    {
        private readonly IDbConnection _connection;
        private readonly IUserIdentificationModel _loggedInUser;
 
        public AccessFeeTransactionRepository(IDbConnection connection, IUserIdentificationModel logedinUser)
        {
            _connection = connection;
            _loggedInUser = logedinUser;
        }

        

        public async Task<IEnumerable<AccessFeeTransactionDataModel>?> GetAll()
        {
            var sql = @"SELECT * FROM accessfeetransaction;";
            return await _connection.QueryAsync<AccessFeeTransactionDataModel>(sql);
        }

        public async Task<AccessFeeTransactionDataModel?> GetById(Guid id)
        {
            var sql = @"SELECT * FROM accessfeetransaction WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            return await _connection.QueryFirstOrDefaultAsync<AccessFeeTransactionDataModel>(sql, parameters);
        }

        public async Task<AccessFeeTransactionDataModel?> Create(AccessFeeTransactionDataModel transaction)
        {
            var sql = @"
                INSERT INTO accessfeetransaction (
                    
                    vehicle_id,
                    amountcharged,
                    vehicle_category,
                    payment_mode,
                    is_active,
                    created_date,
                    created_by,
                    updated_date,
                    updated_by,
                    category_id,
                    ispackagetransaction,
                    packageid
                ) VALUES (
                    
                    @Vehicle_Id,
                    @AmountCharged,
                    @Vehicle_Category,
                    @Payment_Mode,
                    @Is_Active,
                    @Created_Date,
                    @Created_By,
                    @Updated_Date,
                    @Updated_By,
                    @Category_Id,
                    @IsPackageTransaction,
                    @PackageId
                )
                RETURNING *;";

           

            var parameters = new DynamicParameters();
            parameters.Add("@Vehicle_Id", transaction.Vehicle_Id);
            parameters.Add("@AmountCharged", transaction.AmountCharged);
            parameters.Add("@Vehicle_Category", transaction.Vehicle_Category);
            parameters.Add("@Payment_Mode", transaction.Payment_Mode);
            parameters.Add("@Is_Active", transaction.Is_Active);
            parameters.Add("@Created_Date", DateTime.UtcNow);

            //parameters.Add("@Created_Date", DateTime.UtcNow);
            //  parameters.Add("@Created_Date", transaction.Created_Date);
            // parameters.Add("@Created_By", _loggedInUser.Id);
            parameters.Add("@Created_By", "system");
            //parameters.Add("@Created_By", transaction.Created_By);
           // parameters.Add("@Created_Date", DateTime.UtcNow);
            parameters.Add("@Updated_Date", DateTime.UtcNow);
            // parameters.Add("@Updated_By", _loggedInUser.Id);
              parameters.Add("@Updated_By", "system");
            //parameters.Add("@Updated_By", transaction.Updated_By);
            parameters.Add("@Category_Id", transaction.Category_Id);
            parameters.Add("@IsPackageTransaction", transaction.IsPackageTransaction);
            parameters.Add("@PackageId", transaction.PackageId);

            return await _connection.QueryFirstOrDefaultAsync<AccessFeeTransactionDataModel>(sql, parameters);
        }

        public async Task<AccessFeeTransactionDataModel?> Update(AccessFeeTransactionDataModel transaction)
        {
            var sql = @"
                UPDATE accessfeetransaction SET
                    vehicle_id = @Vehicle_Id,
                    amountcharged = @AmountCharged,
                    vehicle_category = @Vehicle_Category,
                    payment_mode = @Payment_Mode,
                    is_active = @Is_Active,
                    updated_by = @Updated_By,
                    updated_date = @Updated_Date,
                    category_id = @Category_Id
                WHERE id = @Id
                RETURNING *;";

            var parameters = new DynamicParameters();
            parameters.Add("@Id", transaction.Id);
            parameters.Add("@Vehicle_Id", transaction.Vehicle_Id);
            parameters.Add("@AmountCharged", transaction.AmountCharged);
            parameters.Add("@Vehicle_Category", transaction.Vehicle_Category);
            parameters.Add("@Payment_Mode", transaction.Payment_Mode);
            parameters.Add("@Is_Active", transaction.Is_Active);
            parameters.Add("@Updated_Date", DateTime.UtcNow);
            // parameters.Add("@Updated_By", _loggedInUser.Id);
            parameters.Add("@Updated_By", transaction.Updated_By);
            parameters.Add("@Category_Id", transaction.Category_Id);

            return await _connection.QueryFirstOrDefaultAsync<AccessFeeTransactionDataModel>(sql, parameters);
        }

        public async Task<bool> Delete(Guid id)
        {
            var sql = @"DELETE FROM accessfeetransaction WHERE id = @Id;";
            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var affected = await _connection.ExecuteAsync(sql, parameters);
            return affected > 0;
        }

        public async Task<TransactionReportResponse?> GetTransactionReport(TransactionReportRequest request)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate.AddDays(1));

            var baseSql = @"
                SELECT 
                    COUNT(*) AS TotalCount, 
                    SUM(amountcharged) AS TotalAmount
                FROM accessfeetransaction
                 WHERE Created_Date >= @FromDate AND Created_Date < @ToDate
                  AND Is_Active = TRUE;";

            var groupSql = @"
                SELECT 
                    TRIM(vehicle_category) AS VehicleCategory,
                    TRIM(payment_mode) AS PaymentMode,
                    COUNT(*) AS Count,
                    SUM(Amountcharged) AS Amount
                FROM accessfeetransaction
                WHERE Created_Date BETWEEN @FromDate AND @ToDate
                  AND Is_Active = TRUE";

            if (!string.IsNullOrWhiteSpace(request.VehicleCategory))
            {
                groupSql += " AND TRIM(LOWER(vehicle_category)) = TRIM(LOWER(@Vehicle_Category))";
                parameters.Add("@Vehicle_Category", request.VehicleCategory.Trim());
            }

            if (!string.IsNullOrWhiteSpace(request.PaymentMode))
            {
                groupSql += " AND TRIM(LOWER(payment_mode)) = TRIM(LOWER(@Payment_Mode))";
                parameters.Add("@Payment_Mode", request.PaymentMode.Trim());
            }

            groupSql += " GROUP BY TRIM(vehicle_category), TRIM(payment_mode) ORDER BY VehicleCategory, PaymentMode;";

            var total = await _connection.QueryFirstOrDefaultAsync<TransactionReportResponse>(baseSql, parameters)
                         ?? new TransactionReportResponse { TotalCount = 0, TotalAmount = 0 };

            var groupedResults = await _connection.QueryAsync<VehicleSummaryDataModel>(groupSql, parameters);
            var summaries = groupedResults.Select(x => new VehicleSummary
            {
                VehicleCategory = x.VehicleCategory,
                PaymentMode = x.PaymentMode,
                Count = x.Count,
                TotalAmount = x.Amount
            }).ToList();

            total.FilteredByVehicleCategory = request.VehicleCategory;
            total.FilteredByPaymentMode = request.PaymentMode;
            total.GroupedSummaries = summaries;

            return total;
        }
    }
}
