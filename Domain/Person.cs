using Domain.Inputs;
using Domain.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Person : ParentEntity
    {
        #region Props
        public int PersonId { get;private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }

        [ForeignKey("Address")]
        public int Address_Id { get; private set; }
        #endregion
        #region VirtualProps
        public Address Address { get; set; }

        #endregion

        #region Domain
        public void Create(CreatePersonInput input)
        {

            #region Validations
            var isValid = true;
            isValid &= string.IsNullOrWhiteSpace(input.Name)
                               ? throw new TaskException(TaskValidationKeysEnum.NameRequired) : true;


            isValid &= input.Age < 0 
                ? throw new TaskException(TaskValidationKeysEnum.AgeRequire) : true;

            
            isValid &= input.AddressId < 0 
                ? throw new TaskException(TaskValidationKeysEnum.AddressRequired) : true;

            
            #endregion


            this.PersonId = input.PersonId;
            this.CreatedBy = input.CreatedBy;
            this.CreationTime = DateTime.Now;
            this.Name = input.Name;
            this.Age = input.Age;
            this.Address_Id = input.AddressId;

        }

        public void Update(CreatePersonInput input)
        {

            #region Validations
            var isValid = true;
            isValid &= string.IsNullOrWhiteSpace(input.Name)
                               ? throw new TaskException(TaskValidationKeysEnum.NameRequired) : true;


            isValid &= input.Age < 0
                ? throw new TaskException(TaskValidationKeysEnum.AgeRequire) : true;


            isValid &= input.AddressId < 0
                ? throw new TaskException(TaskValidationKeysEnum.AddressRequired) : true;


            #endregion


            this.PersonId = input.PersonId;
            this.UpdatedBy = input.UpdatedBy;
            this.UpdateTime = DateTime.Now;
            this.Name = input.Name;
            this.Age = input.Age;
            this.Address_Id = input.AddressId;

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



