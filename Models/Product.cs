using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? ProductName { get; set; }

    public string? Description { get; set; }

    public decimal? Rating { get; set; }

    public decimal? UnitPrice { get; set; }

    public int? StockQuantity { get; set; }

    public string? ProductImage { get; set; }

    public int? ProductCategoryId { get; set; }

    public int? ProviderId { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductCategory? ProductCategory { get; set; }

    public virtual Provider? Provider { get; set; }
}
