using System;
using System.Collections.Generic;

namespace Domain;

public partial class CartItems
{
    public int Id { get; set; }

    public int? CartId { get; set; }

    public int? ItemId { get; set; }

    public int? Quantity { get; set; }

    public virtual Cart? Cart { get; set; }

    public virtual Items? Item { get; set; }
}
