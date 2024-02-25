using System;
using System.ComponentModel.DataAnnotations;

namespace WebMarketplace.Carts;

public class CreateUpdateCartDto
{
    [Required] public Guid UserId { get; set; }
}