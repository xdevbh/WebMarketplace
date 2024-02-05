using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Reviews;

public class ReviewDto : AuditedEntityDto<Guid>
{
    public Guid ProductId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int Rating { get; set; }
    public Guid UserId { get; set; }
}