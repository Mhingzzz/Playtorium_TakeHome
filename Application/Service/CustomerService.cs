using Application.ContractRepo;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class CustomerService : ICustomerService
    {

        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Customers> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerById(id);

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the customer: {ex.Message}");
            }
        }   
        public async Task<Customers> AddCustomerPoint(int id, int point)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerById(id);
                if (customer == null)
                {
                    throw new Exception("Customer not found");
                }

                customer.Points += point;
                await _customerRepository.UpdateCustomer(customer);

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding points: {ex.Message}");
            }
        }


    }
}
