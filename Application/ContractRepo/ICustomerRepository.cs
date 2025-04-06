using Domain;

namespace Application.ContractRepo
{
    public interface ICustomerRepository
    {

        Task<Customers> GetCustomerById(int id);
        Task<Customers> UpdateCustomer(Customers customer);
    }
}
