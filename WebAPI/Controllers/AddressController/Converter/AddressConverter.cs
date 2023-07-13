using Domain;
using Domain.Inputs;
using System;
using WebAPI.Controllers.AddressController.Request;
using WebAPI.Controllers.AddressController.Response;

namespace WebAPI.Controllers.AddressController.Converter
{
    public static class AddressConverter
    {
        public static AddressDetailsResponse ToAddressDetailsResponse(this Address Address)
        {
            return new AddressDetailsResponse
            {
              
                CityNameAr = Address.City.CityNameAr,
                CityNameEn = Address.City.CityNameEn,
                StreetAdress = Address.StreetAdress,
                ZipCode = Address.ZipCode,
            };
        }
        public static CreateAddressInput TocreateAddressInput(this CreateAddressRequest req,
      Func<long> GetAddressKey, long userId)
        {
            return new CreateAddressInput
            {
                AddressId = (int)GetAddressKey(),
              StreetAdress =req.StreetAdress,
              ZipCode=req.ZipCode,
              CityId=req.CityId,
                CreatedBy = userId

            };
        }
        public static CreateAddressInput ToUpdateAddressInput(this CreateAddressRequest req, long userId)
        {
            return new CreateAddressInput
            {
                AddressId =req.AddressId,
                StreetAdress = req.StreetAdress,
                ZipCode = req.ZipCode,
                CityId = req.CityId,
                UpdatedBy = userId

            };
        }

    }
}
