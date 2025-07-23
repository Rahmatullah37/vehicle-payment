using AutoMapper;
using VehicleSurveillance.Domain.Models;
using VehicleSurveillance.Data.Models;
using VehicleSurveillance.Payment.API.Models.CreateRequest;
using VehicleSurveillance.Payment.API.Models.Response;
using VehicleSurveillance.Payment.API.Models.UpdateRequest;

namespace VehicleSurveillance.Payment.API.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<PackageCreateRequestModel, PackageModel>();
            CreateMap<PackageUpdateModel, PackageModel>();
            CreateMap<PackageModel, PackageResponseModel>();

            CreateMap<AccessFeeTransactionCreateRequestModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionUpdateModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionModel, AccessFeeTransactionResponseModel>();

            CreateMap<PaymentModeCreateRequestModel, PaymentModeModel>();
            CreateMap<PaymentModeUpdateModel, PaymentModeModel>();
            CreateMap<PaymentModeModel, PaymentModeResponseModel>();


            CreateMap<TarifCreateRequestModel, TarifModel>();
            CreateMap<TarifUpdateModel, TarifModel>();
            CreateMap<TarifModel, TarifResponseModel>();

            CreateMap<FixedTarifRequestModel, FixedTarifModel>();
            CreateMap<FixedTarifUpdateModel, FixedTarifModel>();
            CreateMap<FixedTarifModel, FixedTarifResponseModel>();

            CreateMap<TarifTypeCreateRequestModel, TarifTypeModel>();
            CreateMap<TarifTypeUpdateModel, TarifTypeModel>();
            CreateMap<TarifTypeModel, TarifTypeResponseModel>();

            CreateMap<VehicleTypeCreateRequestModel, VehicleTypeModel>();
            CreateMap<VehicleTypeUpdateModel, VehicleTypeModel>();
            CreateMap<VehicleTypeModel, VehicleTypeResponseModel>();

            CreateMap<FixedTarifModel, FixedTarifResponseModel>();

            CreateMap<VehicleTarifResponseModel,VehicleTarifModel>();


            CreateMap<AccessFeeTransactionDataModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionModel, AccessFeeTransactionDataModel>();


            CreateMap<PaymentModeModel, PaymentModeDataModel>();
            CreateMap<PaymentModeDataModel, PaymentModeModel>();

            CreateMap<FixedTarifModel, FixedTarifDataModel>();
            CreateMap<FixedTarifDataModel, FixedTarifModel>();

            CreateMap<PackageModel, PackageDataModel>();
            CreateMap<PackageDataModel, PackageModel>();

            CreateMap<TarifModel, TarifDataModel>();
            CreateMap<TarifDataModel, TarifModel>();

            CreateMap<TarifTypeModel, TarifTypeDataModel>();
            CreateMap<TarifTypeDataModel, TarifTypeModel>();

            CreateMap<VehicleTypeModel, VehicleTypeDataModel>();
            CreateMap<VehicleTypeDataModel, VehicleTypeModel>();

            CreateMap<AccessFeeTransactionDataModel, AccessFeeTransactionModel>().ReverseMap();
            CreateMap<VehicleSummary, VehicleSummaryDataModel>();

            CreateMap<HourlyTarifModel, HourlyTarifDataModel>();
            CreateMap<HourlyTarifDataModel, HourlyTarifModel>();
            CreateMap<HourlyTarifRequestModel, HourlyTarifModel>();
            CreateMap<HourlyTarifUpdateModel, HourlyTarifModel>();
            CreateMap<HourlyTarifModel, HourlyTarifResponseModel>();

            CreateMap<TollBoothModel, TollBoothDataModel>();
            CreateMap<TollBoothDataModel, TollBoothModel>();
            CreateMap<TollBoothCreateRequestModel, TollBoothModel>();
            CreateMap<TollBoothUpdateModel, TollBoothModel>();
            CreateMap<TollBoothModel, TollBoothResponseModel>();
        }
    }
}
