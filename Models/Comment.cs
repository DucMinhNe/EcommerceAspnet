using System;
using System.Collections.Generic;

namespace e_commerce_backend.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? CustomerId { get; set; }

    public int? ProductId { get; set; }

    public string? Content { get; set; }

    public int? Rating { get; set; }

    public string? CommentImage { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual Product? Product { get; set; }
}
