using Domain;

namespace Application.ContractRepo
{
    public interface ICartRepository : IBaseRepository<Cart>
    {
        Task<List<Cart>> GetCartByCustomerId(int customerId);
        
        
    }
}
