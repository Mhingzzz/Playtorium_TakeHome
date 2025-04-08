using System.Text.Json.Serialization;

namespace Domain;

public partial class Items
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Category { get; set; }

    public decimal? Price { get; set; }
    [JsonIgnore]
    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();
}
