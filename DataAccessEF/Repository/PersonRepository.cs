using DataAccessEF.Extensions;
using Domain;
using Domain.Filters;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.TypeRepository
{
    class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(PersonDbContext context) : base(context)
        {

        }

        public long GetPersonSequenceKey()
        {
            return context.GetNextSequenceValue("PersonSequenceKey");
        }

        IEnumerable<Person> IPersonRepository.GetAdultPersons()
        {
            return context.Person.Where(pers => pers.Age >= 18).ToList();
        }
      

        IEnumerable<Person> IPersonRepository.GetPersonsFilted(PersonSearchFilter filter)
        {
            var query = context.Person.ToList();
            if (!string.IsNullOrEmpty(filter.Name))
            {
                var q = filter.Name.Trim();
                query.Where(x => x.Name.StartsWith(q));
            }
            if (filter.Age > 0)
                query.Where(x => x.Age >= filter.Age);
            return query;
        }
    }
}

