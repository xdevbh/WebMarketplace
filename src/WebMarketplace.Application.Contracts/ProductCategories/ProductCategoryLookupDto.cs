using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.ProductCategories;

public class ProductCategoryLookupDto: EntityDto<Guid>
{
    public string Name { get; set; }
}