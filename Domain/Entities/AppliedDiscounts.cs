using System.Text.Json.Serialization;

namespace Domain;

public partial class AppliedDiscounts
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public int? CampaignId { get; set; }

    public decimal? DiscountAmount { get; set; }

    public string? DiscountType { get; set; }

    public DateTime? AppliedAt { get; set; }

    [JsonIgnore]
    public virtual DiscountCampaigns? Campaign { get; set; }
    [JsonIgnore]
    public virtual Cart? Cart { get; set; }
}
