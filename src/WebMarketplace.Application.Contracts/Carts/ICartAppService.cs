using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Carts;

public interface ICartAppService : IApplicationService
{
    Task<CartDto> GetAsync();
    Task<CartDto> AddProductAsync(AddCartItemDto input);
    Task<CartDto> ChangeProductQuantityAsync(ChangeCartItemQuantityDto input);
    Task<CartDto> RemoveProductAsync(RemoveCartItemDto input);
    Task<CartDto> ClearAsync();
}