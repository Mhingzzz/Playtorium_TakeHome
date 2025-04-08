using System.Text.Json.Serialization;

namespace Domain;

public partial class Cart
{
    public int Id { get; set; }

    public int CustomerId { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }
    [JsonIgnore]
    public virtual ICollection<AppliedDiscounts> AppliedDiscounts { get; set; } = new List<AppliedDiscounts>();
    [JsonIgnore]
    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
    [JsonIgnore]
    public virtual Customers Customer { get; set; } = null!;
}
