using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AppliedDiscountRepository : BaseRepository<AppliedDiscounts> ,IAppliedDiscountRepository
    {

        public AppliedDiscountRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<bool> IsAlreadyApplyDiscount(int cartId)
        {
            var appliedDiscount = await _dataContext.AppliedDiscounts
                .FirstOrDefaultAsync(x => x.CartId == cartId);
            return appliedDiscount != null;
        }
    }
}
