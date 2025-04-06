using Application.ContractRepo;
using Domain;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class CustomerRepository : BaseRepository<Customers>, ICustomerRepository
    {

        public CustomerRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<Customers> GetCustomerById(int id)
        {
            var result = await GetByIdAsync(id);
            //var result = await _dataContext.Customers.FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<Customers> UpdateCustomer(Customers customer)
        {
            var result = await UpdateAsync(customer);
            return result;
        }
        
    }
}
