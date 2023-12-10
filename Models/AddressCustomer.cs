using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class AddressCustomer
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public string? AddressCustomerName { get; set; }

    public string? PhoneNumber { get; set; }

    public string? City { get; set; }

    public string? District { get; set; }

    public string? SubDistrict { get; set; }

    public string? Address { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
