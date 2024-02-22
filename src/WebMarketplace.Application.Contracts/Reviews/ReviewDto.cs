using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Reviews;

public class ReviewDto : AuditedEntityDto<Guid>
{
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public double Rating { get; set; }
    public string? Content { get; set; }
}