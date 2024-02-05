using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Products;

namespace WebMarketplace.Carts;

public interface ICartItemAppService : ICrudAppService
    <CartItemDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCartItemDto>
{
}