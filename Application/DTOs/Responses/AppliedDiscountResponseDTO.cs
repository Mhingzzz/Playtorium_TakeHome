using Domain;

namespace Application.DTOs.Responses
{
    public class AppliedDiscountSummaryDTO
    {
        public required Cart Cart { get; set; }  
        public decimal DiscountTotal { get; set; }
        public List<AppliedDiscountInfo>? DiscountsApplied { get; set; }
    }

    public class AppliedDiscountInfo
    {
        public string? Type { get; set; }
        public decimal Amount { get; set; }
    }
}