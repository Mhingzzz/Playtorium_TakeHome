using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DiscountCampaignRepository : IDiscountCampaignRepository
    {
        private readonly DataContext _dataContext;
        public DiscountCampaignRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<DiscountCampaigns>> GetActiveCampaign()
        {
            var result = await _dataContext.DiscountCampaigns.Where(x => x.IsActive == true).AsNoTracking().ToListAsync();
            return result;
        }
    }
}
