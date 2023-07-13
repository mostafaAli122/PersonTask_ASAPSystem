using DataAccessEF.TypeRepository;
using Domain.Interfaces;

namespace DataAccessEF.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private PersonDbContext context;
        public UnitOfWork(PersonDbContext context)
        {
            this.context = context;
            Addresses = new AddressRepository(this.context);
            Persons = new PersonRepository(this.context);
            Users = new UserRepository(this.context);
        }

        public IAdressRepository Addresses { get; private set; }
        public IPersonRepository Persons { get; private set; }
        public IUserRepository Users { get; private set; }

        public void Dispose()
        {
            context.Dispose();
        }

        public int Save()
        {
            return context.SaveChanges();
        }
    }
}
