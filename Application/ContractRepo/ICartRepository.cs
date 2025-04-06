using Domain;

namespace Application.ContractRepo
{
    public interface ICartRepository
    {
        Task<List<Cart>> GetCartByCustomerId(int customerId);
        Task<Cart> CreateCart(Cart cart);
        
    }
}
