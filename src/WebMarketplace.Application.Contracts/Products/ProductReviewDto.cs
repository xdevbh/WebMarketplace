using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;

namespace WebMarketplace.Products;

public class ProductReviewDto : EntityDto<Guid>, IHasCreationTime
{
    public string UserName { get; set; }
    public Guid ProductId { get; set; }
    public double Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreationTime { get; set; }
}