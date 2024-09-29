using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebMarketplace.Products;

public class ProductSellerAppService : WebMarketplaceAppService, IProductSellerAppService
{
    public Task<ProductDto> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<ProductDto>> GetListAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}