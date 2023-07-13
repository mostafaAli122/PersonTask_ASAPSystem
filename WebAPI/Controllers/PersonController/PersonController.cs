using DataAccessEF.Services.LogFile;
using Domain;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using WebAPI.Controllers.PersonController.Converter;
using WebAPI.Controllers.PersonController.Request;
using WebAPI.Controllers.PersonController.Response;
using Domain.Shared;
using System;

namespace WebAPI.Controllers.PersonController
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class PersonController : TaskBaseController
    {

        private readonly IUnitOfWork unitOfWork;

        public PersonController(IUnitOfWork unitOfWork, LogFileService logger) :base(logger)
        {
            this.unitOfWork = unitOfWork;
        }
        #region Query
        [HttpGet]
        [Route("GetAllPersons")]

        public DescriptiveResponse<IEnumerable<Person>> GetAllPersons()
        {
            return this.TryCatchLog(() =>
            {
                return unitOfWork.Persons.GetAll();
            });
        }
        [HttpGet]
        [Route("GetPersonsFiltedList")]

        public DescriptiveResponse<IEnumerable<Person>> GetPersonsFiltedList(PersonSearchFilter filter)
        {
            return this.TryCatchLog(() =>
            {
                return unitOfWork.Persons.GetPersonsFilted(filter);
            });
        }

        [HttpGet]
        [Route("GetPersonDetails")]
        public DescriptiveResponse<PersonDetailsResponse> GetSLADetails(int id)
        {
            return TryCatchLog(() =>
            {

                var person = this.unitOfWork.Persons.GetById(id)
                    ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                return person.ToPersonDetailsResponse();
            });
        }

        #endregion

        #region Command
        [HttpPost("AddPerson")]
        public DescriptiveResponse<bool> AddPerson(CreatePersonRequest request)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();

                var person = new Person();
                this.unitOfWork.Persons.Add(person);

                var input = request.TocreatePersonInput(this.unitOfWork.Persons.GetPersonSequenceKey,userId);
                person.Create(input);

                this.unitOfWork.Save();

                return true;
            });
        }
        [HttpPost("DeletePerson")]
        public DescriptiveResponse<bool> DeletePerson([FromBody] int Id)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();
                var person = this.unitOfWork.Persons.GetById(Id)
                   ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                person.softDelete(userId);
                this.unitOfWork.Save();

                return true;
            });
        }



        [HttpPost("UpdatePerson")]
        public DescriptiveResponse<bool> UpdatePerson(CreatePersonRequest request)
        {
            return this.TryCatchLog(() =>
            {
                var userId = GetUserId_Token();

                var person = this.unitOfWork.Persons.GetById(request.PersonId)
                     ?? throw new TaskException(TaskValidationKeysEnum.NotFound);

                var input = request.ToUpdatePersonInput( userId);
                person.Update(input);

                this.unitOfWork.Save();

                return true;
            });
        }


        #endregion

    }
}
