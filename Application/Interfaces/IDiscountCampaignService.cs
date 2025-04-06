using Domain;

namespace Application.Interfaces
{
    public interface IDiscountCampaignService
    {
        decimal ApplyFixedDiscount(decimal total, decimal discount);
        Task<List<DiscountCampaigns>> GetActiveCampaign();
    }
}
