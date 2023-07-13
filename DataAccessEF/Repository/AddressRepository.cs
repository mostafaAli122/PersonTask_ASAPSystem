using DataAccessEF.Extensions;
using Domain;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessEF.TypeRepository
{
    class AddressRepository : GenericRepository<Address>, IAdressRepository
    {
        public AddressRepository(PersonDbContext context) : base(context)
        {

        }
        IEnumerable<Address> IAdressRepository.GetAddressesFilted(Domain.Filters.AddressSearchFilter filter)
        {
            var query = context.Address.ToList();
            if (!string.IsNullOrEmpty(filter.StreetAdress))
            {
                var q = filter.StreetAdress.Trim();
                query.Where(x => x.StreetAdress.StartsWith(q));
            }
            if (!string.IsNullOrEmpty(filter.ZipCode))
            {
                var q = filter.ZipCode.Trim();
                query.Where(x => x.ZipCode.StartsWith(q));
            }
           
            return query;
        }

        public long GetAddressSequenceKey()
        {
            return context.GetNextSequenceValue("AddressSequenceKey");
        }
    }
}

