using Domain.Enum;

namespace Application.DTOs.Requests
{
    public class DiscountRequest
    {
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
    }

    public class DiscountCategoryRequest
    {
        public required int CampaignIds { get; set; }
        public DiscountCategoryType DiscountCategoryType { get; set; }
    }

}
