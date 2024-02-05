using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Carts;

public class CartItemAppService : CrudAppService
    <CartItem, CartItemDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCartItemDto>
{
    public CartItemAppService(IRepository<CartItem, Guid> repository)
        : base(repository)
    {
        
    }
}