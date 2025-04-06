using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ContractRepo
{
    public interface IDiscountCampaignRepository
    {
        Task<List<DiscountCampaigns>> GetActiveCampaign();
    }
}
