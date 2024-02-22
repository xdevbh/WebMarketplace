using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace WebMarketplace.Orders;

public class Order: AuditedAggregateRoot<Guid>
{
    public Guid UserId { get; set; }
    public OrderStatus Status { get; set; } 
}