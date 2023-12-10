using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
