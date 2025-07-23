using System;
using System.Collections.Generic;
using VehicleSurveillance.Domain.Models;

namespace VehicleSurveillance.Services.Interfaces
{
    public interface IPaymentModeService
    {
        List<PaymentModeModel> GetAll();
        PaymentModeModel GetById(Guid id);
        void Add(PaymentModeModel mode);
        void Update(PaymentModeModel mode);
        void Delete(Guid id);
    }
}
