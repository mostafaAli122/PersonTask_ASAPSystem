using Domain.Inputs;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Address:ParentEntity
    {
        #region Props
          public int AddressId { get; private set; }
          public string StreetAdress { get; private set; }
          public string ZipCode { get; private set; }
          
          [ForeignKey("City")]
          public int CityId { get; private set; }
        #endregion

        #region Virtual Props
        public City City { get; set; }
        #endregion

        #region Domain
        public void Create(CreateAddressInput input)
        {

            #region Validations
            var isValid = true;
            isValid &= string.IsNullOrWhiteSpace(input.StreetAdress)
                               ? throw new TaskException(TaskValidationKeysEnum.StreetAdressRequired) : true;

               isValid &= string.IsNullOrWhiteSpace(input.ZipCode)
                               ? throw new TaskException(TaskValidationKeysEnum.ZipCodeRequired) : true;


        

            isValid &= input.CityId < 0
                ? throw new TaskException(TaskValidationKeysEnum.CityIdRequired) : true;


            #endregion


            this.AddressId = input.AddressId;
            this.CreatedBy = input.CreatedBy;
            this.CreationTime = DateTime.Now;
            this.StreetAdress = input.StreetAdress;
            this.ZipCode = input.ZipCode;
            this.CityId = input.CityId;

        }

        public void Update(CreateAddressInput input)
        {

            #region Validations
            var isValid = true;
            isValid &= string.IsNullOrWhiteSpace(input.StreetAdress)
                               ? throw new TaskException(TaskValidationKeysEnum.StreetAdressRequired) : true;

            isValid &= string.IsNullOrWhiteSpace(input.ZipCode)
                            ? throw new TaskException(TaskValidationKeysEnum.ZipCodeRequired) : true;




            isValid &= input.CityId < 0
                ? throw new TaskException(TaskValidationKeysEnum.CityIdRequired) : true;



            #endregion


            this.AddressId = input.AddressId;
            this.UpdatedBy = input.UpdatedBy;
            this.UpdateTime = DateTime.Now;
            this.StreetAdress = input.StreetAdress;
            this.ZipCode = input.ZipCode;
            this.CityId = input.CityId;

        }
        public void softDelete(long userId)
        {
            this.IsDeleted = true;
            this.UpdatedBy = userId;
            this.UpdateTime = DateTime.Now;
        }
        #endregion


    }
}
