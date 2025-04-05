using System;
using System.Collections.Generic;

namespace Domain;

public partial class Cart
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public decimal? TotalPrice { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<AppliedDiscounts> AppliedDiscounts { get; set; } = new List<AppliedDiscounts>();

    public virtual ICollection<CartItems> CartItems { get; set; } = new List<CartItems>();

    public virtual Customers? Customer { get; set; }
}
