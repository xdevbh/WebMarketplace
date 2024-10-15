using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class SimilarProductCardListFilterDto : PagedResultRequestDto
{
    public Guid ProductId { get; set; }
}