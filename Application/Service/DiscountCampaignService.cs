using Application.ContractRepo;
using Application.Interfaces;
using Domain;

namespace Application.Service
{
    public class DiscountCampaignService : IDiscountCampaignService
    {

        private readonly IDiscountCampaignRepository _discountCampaignRepository;
        public DiscountCampaignService(IDiscountCampaignRepository discountCampaignRepository)
        {
            _discountCampaignRepository = discountCampaignRepository;
        }
        public decimal ApplyFixedDiscount(decimal total, decimal discount)
        {
            if (total < 0)
            {
                throw new ArgumentException("Total amount cannot be negative.");
            }

            if (discount < 0)
            {
                throw new ArgumentException("Discount cannot be negative.");
            }
            return Math.Max(0, total - discount);
        }

        public async Task<List<DiscountCampaigns>> GetActiveCampaign()
        {
            try
            {
                var campaigns = await _discountCampaignRepository.GetActiveCampaign();
                return campaigns;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving active campaigns: {ex.Message}");
            }
        }

        

    }
}
