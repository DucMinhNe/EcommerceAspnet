using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class Customer
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? BirthDate { get; set; }

    public bool? Gender { get; set; }

    public string? CustomerImage { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<AddressCustomer> AddressCustomers { get; set; } = new List<AddressCustomer>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
