using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Products;

public interface IProductAdminAppService : IApplicationService
{
    Task DeleteReview(Guid productId, Guid reviewId); // todo 
    Task DeleteProduct(Guid id);
}