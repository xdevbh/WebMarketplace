using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Products;

public class ProductAppService : CrudAppService
    <Product, ProductDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateProductDto>
{
    public ProductAppService(IRepository<Product, Guid> repository)
            : base(repository)
    {
    }
}