using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Authorization;
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
        else
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
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
        else
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        return new CartDto();
    }
    
    public async Task<CartDto> ChangeProductQuantityAsync(ChangeCartItemQuantityDto input)
    {
        if (CurrentUser.IsAuthenticated)
        {
            var userCart = await _cartRepository.GetAsync(CurrentUser.GetId());
            userCart.ChangeItemQuantity(input.ProductId, input.Quantity);
            await _cartRepository.UpdateAsync(userCart);
            
            var dto = await GetCartDtoAsync(userCart);
            return dto;
        }
        else
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
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
        else
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        return new CartDto();
    }
    
    private async Task<CartDto> GetCartDtoAsync(Cart cart)
    {
        var dto = new CartDto();

        foreach (var item in cart.Items)
        {
            var product = await _productRepository.GetProductDetailAsync(item.ProductId);
            var cartItemDto = new CartItemDto();
            cartItemDto.ProductId = product.Id;
            cartItemDto.ProductName = product.Name;
            cartItemDto.CompanyId = product.CompanyId;
            cartItemDto.CompanyName = product.CompanyName;
            cartItemDto.Quantity = item.Quantity;
            cartItemDto.UnitPrice = product.PriceAmount;
            cartItemDto.TotalPrice = item.Quantity * product.PriceAmount;
            cartItemDto.Currency = product.PriceCurrency;
            dto.Items.Add(cartItemDto);
        }
        
        dto.TotalPrice = dto.Items.Sum(x => x.TotalPrice);
        return dto;
    }
    
    public async Task<CartDto> ClearAsync()
    {
        if (CurrentUser.IsAuthenticated)
        {
            var userCart = await _cartRepository.GetAsync(CurrentUser.GetId());
            userCart.Clear();
            await _cartRepository.UpdateAsync(userCart);
            
            var dto = await GetCartDtoAsync(userCart);
            return dto;
        }
        else
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        return new CartDto();
    }
}