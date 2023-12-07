using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class JobTitle
{
    public int Id { get; set; }

    public string? JobTitleName { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
