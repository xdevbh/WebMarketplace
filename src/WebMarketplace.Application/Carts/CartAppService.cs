using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Products;

namespace WebMarketplace.Carts;

public class CartAppService : CrudAppService
    <Cart, CartDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCartDto>, ICartAppService
{
    public CartAppService(IRepository<Cart, Guid> repository)
        : base(repository)
    {
        
    }
    
}