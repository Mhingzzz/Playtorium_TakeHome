namespace Application.DTOs.Requests
{
    public class AddItemToCartRequest
    {
        public int CartId { get; set; }
        public required int ItemId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateCartItemRequest
    {
        public int CartItemId { get; set; }
        public int Quantity { get; set; }
    }

}