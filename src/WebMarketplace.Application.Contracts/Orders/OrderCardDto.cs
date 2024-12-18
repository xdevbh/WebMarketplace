﻿using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace WebMarketplace.Orders;

public class OrderCardDto : CreationAuditedEntityDto<Guid>
{
    public OrderStatus Status { get; set; }
    public decimal TotalPrice { get; set; }
    public string Currency { get; set; }
}