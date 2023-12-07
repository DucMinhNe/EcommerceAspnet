using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string? PaymentMethodName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
