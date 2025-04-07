using Application.ContractRepo;
using Domain;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class DiscountCampaignRepository : BaseRepository<DiscountCampaigns>, IDiscountCampaignRepository
    {
        public DiscountCampaignRepository(DataContext dataContext) : base(dataContext)
        {
        }

        public async Task<List<DiscountCampaigns>> GetActiveCampaign()
        {
            var result = await _dataContext.DiscountCampaigns.Where(x => x.IsActive == true).AsNoTracking().ToListAsync();
            return result;
        }
    }
}
