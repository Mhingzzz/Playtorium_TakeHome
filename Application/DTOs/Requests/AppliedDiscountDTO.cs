namespace Application.DTOs.Requests
{
    public class AppliedDiscountRequestDTO
    {
        public int CartId { get; set; }
        public required DiscountCategoryRequest[] DiscountCategoryRequest { get; set; }

    }


}
