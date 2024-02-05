using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Carts;

public class CartAppService : CrudAppService
    <Cart, CartDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCartDto>
{
    public CartAppService(IRepository<Cart, Guid> repository)
        : base(repository)
    {
        
    }
}