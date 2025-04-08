using Application.ContractRepo;
using Domain;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class AppliedDiscountRepository : BaseRepository<AppliedDiscounts> ,IAppliedDiscountRepository
    {

        public AppliedDiscountRepository(DataContext dataContext) : base(dataContext)
        {
        }

        
    }
}
