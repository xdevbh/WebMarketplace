using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductAdminAppService : WebMarketplaceAppService, IProductAdminAppService
{
    public Task DeleteReview(Guid productId, Guid reviewId)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(Guid id)
    {
        throw new NotImplementedException();
    }
    
    // can edit product with vendor ID
}