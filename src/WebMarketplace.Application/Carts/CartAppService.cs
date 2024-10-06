using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Users;
using WebMarketplace.Products;

namespace WebMarketplace.Carts;

public class CartAppService : WebMarketplaceAppService, ICartAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ICartRepository _cartRepository;

    public CartAppService(IProductRepository productRepository, ICartRepository cartRepository)
    {
        _productRepository = productRepository;
        _cartRepository = cartRepository;
    }

    public async Task<CartDto> GetAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            var userCart = await _cartRepository.GetAsync(CurrentUser.GetId());
            var dto = await GetCartDtoAsync(userCart);
            return dto;
        }

        return new CartDto();
    }

    public async Task<CartDto> AddProductAsync(AddCartItemDto input)
    {
        if (CurrentUser.IsAuthenticated)
        {
            var userCart = await _cartRepository.GetAsync(CurrentUser.GetId());
            userCart.AddItem(input.ProductId, input.Quantity);
            await _cartRepository.UpdateAsync(userCart);
            
            var dto = await GetCartDtoAsync(userCart);
            return dto;
        }

        return new CartDto();
    }

    public async Task<CartDto> RemoveProductAsync(RemoveCartItemDto input)
    {
        if (CurrentUser.IsAuthenticated)
        {
            var userCart = await _cartRepository.GetAsync(CurrentUser.GetId());
            userCart.RemoveItem(input.ProductId, input.Quantity);
            await _cartRepository.UpdateAsync(userCart);
            var dto = await GetCartDtoAsync(userCart);
            return dto;
        }

        return new CartDto();
    }
    
    private async Task<CartDto> GetCartDtoAsync(Cart cart)
    {
        var dto = new CartDto();

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetAsync(item.ProductId);
            var cartItemDto = new CartItemDto();
            cartItemDto.ProductId = product.Id;
            cartItemDto.ProductName = product.Name;
            cartItemDto.Quantity = item.Quantity;
            cartItemDto.TotalPrice = item.Quantity * product.CurrentPrice?.Amount ?? 0;
            dto.Items.Add(cartItemDto);
        }
        
        dto.TotalPrice = dto.Items.Sum(x => x.TotalPrice);
        return dto;
    }
}