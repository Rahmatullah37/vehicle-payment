using AutoMapper;
using VisualSoft.Surveillance.Payment.Domain.Models;
using VisualSoft.Surveillance.Payment.Data.Models;
using VisualSoft.Surveillance.Payment.API.Models.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.Response;
using VisualSoft.Surveillance.Payment.API.Models.UpdateRequest;
using VisualSoft.Surveillance.Payment.API.Models.VehicleAccount.CreateRequest;
using VisualSoft.Surveillance.Payment.API.Models.VehicleAccount.UpdateRequest;
using VisualSoft.Surveillance.Payment.API.Models.VehicleAccount.Response;

namespace VisualSoft.Surveillance.Payment.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PackageCreateRequestModel, PackageModel>();
            CreateMap<PackageUpdateModel, PackageModel>();
            CreateMap<PackageModel, PackageResponseModel>();

            CreateMap<AccessFeeTransactionCreateRequestModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionUpdateModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionModel, AccessFeeTransactionResponseModel>();

            CreateMap<AccessFeeTransactionDataModel, AccessFeeTransactionModel>();
            CreateMap<AccessFeeTransactionModel, AccessFeeTransactionDataModel>();

            CreateMap<PaymentModeCreateRequestModel, PaymentModeModel>();
            CreateMap<PaymentModeUpdateModel, PaymentModeModel>();
            CreateMap<PaymentModeModel, PaymentModeResponseModel>();


            CreateMap<TarifCreateRequestModel, TarifModel>();
            CreateMap<TarifUpdateModel, TarifModel>();
            CreateMap<TarifModel, TarifResponseModel>();

            CreateMap<FixedTarifRequestModel, FixedTarifModel>();
            CreateMap<FixedTarifUpdateModel, FixedTarifModel>();
            CreateMap<FixedTarifModel, FixedTarifResponseModel>();

            CreateMap<FixedTarifModel, FixedTarifDataModel>();
            CreateMap<FixedTarifDataModel, FixedTarifModel>();

            CreateMap<TarifTypeCreateRequestModel, TarifTypeModel>();
            CreateMap<TarifTypeUpdateModel, TarifTypeModel>();
            CreateMap<TarifTypeModel, TarifTypeResponseModel>();


            CreateMap<VehicleTypeCreateRequestModel, VehicleTypeModel>();
            CreateMap<VehicleTypeUpdateModel, VehicleTypeModel>();
            CreateMap<VehicleTypeModel, VehicleTypeResponseModel>();


            CreateMap<VehicleTarifResponseModel, VehicleTarifModel>();




            CreateMap<PaymentModeModel, PaymentModeDataModel>();
            CreateMap<PaymentModeDataModel, PaymentModeModel>();



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

            CreateMap<VehiclePackageRequestModel, VehiclePackageModel>();
            CreateMap<VehiclePackageUpdateModel, VehiclePackageModel>();
            CreateMap<VehiclePackageModel, VehiclePackageResponseModel>();


            CreateMap<VehiclePackageModel, VehiclePackageDataModel>();
            CreateMap<VehiclePackageDataModel, VehiclePackageModel>();



            // Vehicle Account mappings - Add these to your MappingProfile constructor
            CreateMap<VehicleAccountCreateRequestModel, VehicleAccountModel>();
            CreateMap<VehicleAccountUpdateModel, VehicleAccountModel>();
            CreateMap<VehicleAccountModel, VehicleAccountResponseModel>();

            CreateMap<VehicleAccountModel, VehicleAccountDataModel>();
            CreateMap<VehicleAccountDataModel, VehicleAccountModel>();

            CreateMap<FundTransferRequestModel, VehicleAccountModel>();
            CreateMap<PaymentRequestModel, VehicleAccountModel>();

            // If you have separate transaction models
            CreateMap<VehicleAccountModel, VehicleAccountDataModel>();
            CreateMap<VehicleAccountDataModel, VehicleAccountModel>();

            CreateMap<VehicleAccountCreateRequestModel, VehicleAccountModel>();
            CreateMap<VehicleAccountUpdateModel, VehicleAccountModel>();


            CreateMap<VehicleHistoryDomainModel, VehicleAccountDomainModel>();
            CreateMap<VehicleAccountDomainModel, VehicleAccountResponseModel>();
            CreateMap<VehicleTransactionHistoryModel, VehicleTransactionHistoryViewModel>();

            CreateMap<PackagePopularityDataModel, PackagePopularityDomainModel>();
            CreateMap<PackagePopularityDomainModel, PackagePopularityResponseModel>();
        }
    }
}
