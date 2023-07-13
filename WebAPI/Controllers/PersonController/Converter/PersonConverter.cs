using Domain;
using Domain.Inputs;
using System;
using WebAPI.Controllers.PersonController.Request;
using WebAPI.Controllers.PersonController.Response;

namespace WebAPI.Controllers.PersonController.Converter
{
    public static class PersonConverter
    {
      public static PersonDetailsResponse ToPersonDetailsResponse (this Person person)
        {
            return new PersonDetailsResponse
            {
                Name = person.Name,
                Age = person.Age,
                CityNameAr = person.Address.City.CityNameAr,
                CityNameEn = person.Address.City.CityNameEn,
                StreetAdress = person.Address.StreetAdress,
                ZipCode = person.Address.ZipCode,
            };
        }
        public static CreatePersonInput TocreatePersonInput(this CreatePersonRequest req,
      Func<long> GetPersonKey,long userId )
        {
            return new CreatePersonInput
            {
                PersonId =(int) GetPersonKey(),
                Age = req.Age,
                Name = req.Name,
                AddressId = req.AddressId,
                CreatedBy=userId

            };
        }  
        public static CreatePersonInput ToUpdatePersonInput(this CreatePersonRequest req,long userId )
        {
            return new CreatePersonInput
            {
                PersonId =req.PersonId,
                Age = req.Age,
                Name = req.Name,
                AddressId = req.AddressId,
                UpdatedBy=userId

            };
        }

    }

}
