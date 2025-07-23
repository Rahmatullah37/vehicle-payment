using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public class AccessFeeTransactionRepository:IAccessFeeTransactionRepository
    {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public AccessFeeTransactionRepository(IDbConnection connection, IDbTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        public List<AccessFeeTransactionDataModel> GetAll()
        {
            return _connection.Query<AccessFeeTransactionDataModel>("SELECT * FROM accessfeetransaction", transaction: _transaction)
                .ToList();
        }

        public AccessFeeTransactionDataModel GetById(Guid id)
        {
            return _connection
                .QueryFirstOrDefault<AccessFeeTransactionDataModel>(
                    "SELECT * FROM accessfeetransaction WHERE id = @Id",
                    new { Id = id },
                    transaction: _transaction
                );
        }

        public void Create(AccessFeeTransactionDataModel transactionModel)
        {
            _connection.Execute(
                @"INSERT INTO accessfeetransaction 
                (vehicle_id, amountcharged, vehicle_category, payment_mode, is_active, created_date, created_by, updated_date, updated_by,category_id)
                VALUES 
                (@Vehicle_Id, @AmountCharged, @Vehicle_Category, @Payment_Mode, @Is_Active, @Created_Date, @Created_By, @Updated_Date, @Updated_By,@Category_Id)",
                transactionModel,
                transaction: _transaction
            );
        }

        public void Update(AccessFeeTransactionDataModel transactionModel)
        {
            _connection.Execute(
                @"UPDATE accessfeetransaction SET
                    vehicle_id = @Vehicle_Id,
                    amountcharged = @AmountCharged,
                    vehicle_category = @Vehicle_Category,
                    payment_mode = @Payment_Mode,
                    is_active = @Is_Active,
                    updated_by = @Updated_By,
                    updated_date = @Updated_Date,
                    category_id=@Category_Id
                  WHERE id = @Id",
                transactionModel,
                transaction: _transaction
            );
        }

        public void Delete(Guid id)
        {
            _connection.Execute(
                "DELETE FROM accessfeetransaction WHERE id = @Id",
                new { Id = id },
                transaction: _transaction
            );
        }
        public TransactionReportResponse GetTransactionReport(TransactionReportRequest request)
        {
            // Prepare parameters
            var parameters = new DynamicParameters();
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate); //AddDays(1).AddTicks(-1) ensures the full day is covered up to 23:59:59.

            //  Base SQL for total count and amount
                string baseSql = @"
            SELECT 
                COUNT(*) AS TotalCount, 
                SUM(Amountcharged) AS TotalAmount
            FROM accessfeetransaction
            WHERE Created_Date BETWEEN @FromDate AND @ToDate
              AND Is_Active = TRUE";

            //  Grouped SQL for summaries (start with basic query)
                string groupSql = @"
            SELECT 
                TRIM(vehicle_category) AS VehicleCategory,
                TRIM(payment_mode) AS PaymentMode,
                COUNT(*) AS Count,
                SUM(Amountcharged) AS Amount
            FROM accessfeetransaction
            WHERE Created_Date BETWEEN @FromDate AND @ToDate
              AND Is_Active = TRUE";

            // Add optional filters (VehicleCategory and PaymentMode)
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

            //  Finish the grouped SQL with GROUP BY
            groupSql += " GROUP BY TRIM(vehicle_category), TRIM(payment_mode) ORDER BY VehicleCategory, PaymentMode;";

            // Execute queries
            var totalResult = _connection.QueryFirstOrDefault<TransactionReportResponse>(
                baseSql, parameters, transaction: _transaction);

            var total = totalResult ?? new TransactionReportResponse
            {
                TotalCount = 0,
                TotalAmount = 0
            };

            var groupedResults = _connection.Query<VehicleSummaryDataModel>(
                groupSql, parameters, transaction: _transaction)?.ToList()
                ?? new List<VehicleSummaryDataModel>();

            //  Map grouped data to summaries
            var summaries = groupedResults.Select(x => new VehicleSummary
            {
                VehicleCategory = x.VehicleCategory,
                PaymentMode = x.PaymentMode,
                Count = x.Count,
                TotalAmount = x.Amount
            }).ToList();

            //  Fill filters and grouped data in final result
            total.FilteredByVehicleCategory = request.VehicleCategory;
            total.FilteredByPaymentMode = request.PaymentMode;
            total.GroupedSummaries = summaries;

            //  Return the result
            return total;
        }


    }
}
