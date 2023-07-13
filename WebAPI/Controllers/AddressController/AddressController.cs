using DataAccessEF.Services.LogFile;
using Domain;
using Domain.Filters;
using Domain.Interfaces;
using Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI.Controllers.AddressController.Converter;
using WebAPI.Controllers.AddressController.Request;
using WebAPI.Controllers.AddressController.Response;

namespace WebAPI.Controllers.AddressController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class AddressController : TaskBaseController
    {

        private readonly IUnitOfWork unitOfWork;

        public AddressController(IUnitOfWork unitOfWork, LogFileService logger) : base(logger)
        {
            this.unitOfWork = unitOfWork;
        }
        #region Query
        [HttpGet]
        [Route("GetAllAddresses")]

        public DescriptiveResponse<IEnumerable<Address>> GetAllAddresses()
        {
            return this.TryCatchLog(() =>
            {
                return unitOfWork.Addresses.GetAll();
            });
        }
        [HttpGet]
        [Route("GetAddresssFiltedList")]

        public DescriptiveResponse<IEnumerable<Address>> GetAddresssFiltedList(AddressSearchFilter filter)
        {
            return this.TryCatchLog(() =>
            {
                return unitOfWork.Addresses.GetAddressesFilted(filter);
            });
        }

        [HttpGet]
        [Route("GetAddressDetails")]
        public DescriptiveResponse<AddressDetailsResponse> GetSLADetails(int id)
        {
            return TryCatchLog(() =>
            {

                var Address = this.unitOfWork.Addresses.GetById(id)
                    ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                return Address.ToAddressDetailsResponse();
            });
        }

        #endregion

        #region Command
        [HttpPost("AddAddress")]
        public DescriptiveResponse<bool> AddAddress(CreateAddressRequest request)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();

                var Address = new Address();
                this.unitOfWork.Addresses.Add(Address);

                var input = request.TocreateAddressInput(this.unitOfWork.Addresses.GetAddressSequenceKey, userId);
                Address.Create(input);

                this.unitOfWork.Save();

                return true;
            });
        }
        [HttpPost("DeleteAddress")]
        public DescriptiveResponse<bool> DeleteAddress([FromBody] int Id)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();
                var Address = this.unitOfWork.Addresses.GetById(Id)
                   ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                Address.softDelete(userId);
                this.unitOfWork.Save();

                return true;
            });
        }



        [HttpPost("UpdateAddress")]
        public DescriptiveResponse<bool> UpdateAddress(CreateAddressRequest request)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();

                var Address = this.unitOfWork.Addresses.GetById(request.AddressId)
                     ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                var input = request.ToUpdateAddressInput(userId);
                Address.Update(input);

                this.unitOfWork.Save();

                return true;
            });
        }


        #endregion

    }
}
