using System.Text.Json.Serialization;

namespace Domain;

public partial class Customers
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public int? Points { get; set; }

    [JsonIgnore]  // This will prevent serialization of the Cart property
    public virtual ICollection<Cart> Cart { get; set; } = new List<Cart>();
}
