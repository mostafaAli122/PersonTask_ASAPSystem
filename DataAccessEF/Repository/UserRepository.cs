using Domain;
using Domain.Filters;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using DataAccessEF.Extensions;

namespace DataAccessEF.TypeRepository
{
    class UserRepository : GenericRepository<IdentityUser>, IUserRepository
    {
        public UserRepository(PersonDbContext context) : base(context)
        {

        }

        public long GetUserGeneratedTokenKey()
        {
            return context.GetNextSequenceValue("UserGeneratedTokenSequence");
        }
    }
}

