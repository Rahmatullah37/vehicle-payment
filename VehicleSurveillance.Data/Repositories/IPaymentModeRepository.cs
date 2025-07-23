using System;
using System.Collections.Generic;
using VehicleSurveillance.Data.Models;

namespace VehicleSurveillance.Data.Repositories
{
    public interface IPaymentModeRepository
    {
        List<PaymentModeDataModel> GetAll();
        PaymentModeDataModel GetById(Guid id);
        void Create(PaymentModeDataModel mode);
        void Update(PaymentModeDataModel mode);
        void Delete(Guid id);
        PaymentModeDataModel GetPaymentByName(string name);
    }
}
