using System.Text.Json.Serialization;

namespace Domain;

public partial class DiscountCampaigns
{
    public int Id { get; set; }

    public string? CampaignType { get; set; }

    public decimal? DiscountValue { get; set; }

    public string? Category { get; set; }

    public string? ItemCategory { get; set; }

    public decimal? PointsCap { get; set; }

    public decimal? EveryXThb { get; set; }

    public decimal? DiscountYThb { get; set; }

    public bool? IsActive { get; set; }
    [JsonIgnore]
    public virtual ICollection<AppliedDiscounts> AppliedDiscounts { get; set; } = new List<AppliedDiscounts>();
}
