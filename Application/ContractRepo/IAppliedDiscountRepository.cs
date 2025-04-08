using Domain;

namespace Application.ContractRepo
{
    public interface IAppliedDiscountRepository : IBaseRepository<AppliedDiscounts>
    {
        Task<bool> IsAlreadyApplyDiscount(int cartId);
    }
}
