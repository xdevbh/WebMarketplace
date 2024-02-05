using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Carts;

public class CartDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
}