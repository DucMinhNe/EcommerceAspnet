using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class ProductCategory
{
    public int Id { get; set; }

    public string? ProductCategoryName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
