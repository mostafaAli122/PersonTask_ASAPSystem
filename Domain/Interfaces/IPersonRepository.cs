using Domain.Filters;
using System.Collections.Generic;

namespace Domain.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        IEnumerable<Person> GetAdultPersons();
        IEnumerable<Person> GetPersonsFilted(PersonSearchFilter filter);
        long GetPersonSequenceKey();

    }
}
