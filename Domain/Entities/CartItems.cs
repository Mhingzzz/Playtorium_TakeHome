using System.Text.Json.Serialization;

namespace Domain;

public partial class CartItems
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    [JsonIgnore]
    public virtual Cart? Cart { get; set; }

    [JsonIgnore]
    public virtual Items? Item { get; set; }
}
