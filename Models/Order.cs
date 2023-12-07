using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? EmployeeId { get; set; }

    public DateTime? OrderDateTime { get; set; }

    public int? AddressCustomerId { get; set; }

    public decimal? TotalPrice { get; set; }

    public decimal? ShippingCost { get; set; }

    public int? PaymentMethodId { get; set; }

    public string? OrderStatus { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual AddressCustomer? AddressCustomer { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PaymentMethod? PaymentMethod { get; set; }
}
