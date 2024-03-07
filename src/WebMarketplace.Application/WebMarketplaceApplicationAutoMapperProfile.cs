using AutoMapper;
using WebMarketplace.Carts;
using WebMarketplace.Currencies;
using WebMarketplace.Orders;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Reviews;
using WebMarketplace.UserVendors;
using WebMarketplace.Vendors;

namespace WebMarketplace;

public class WebMarketplaceApplicationAutoMapperProfile : Profile
{
    public WebMarketplaceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<Product, ProductViewDto>();
        
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, ProductCategoryLookupDto>();
        CreateMap<CreateUpdateProductCategoryDto, ProductCategory>();
        
        CreateMap<Cart, CartDto>();
        CreateMap<CreateUpdateCartDto, Cart>();
        
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CreateUpdateCartItemDto, CartItem>();
        
        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>();
        
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<CreateUpdateOrderItemDto, OrderItem>();
        
        CreateMap<Vendor, VendorDto>();
        CreateMap<CreateUpdateVendorDto, Vendor>();
        
        CreateMap<UserVendor, UserVendorDto>();
        CreateMap<CreateUpdateUserVendorDto, UserVendor>();
        
        CreateMap<Review, ReviewDto>();
        CreateMap<CreateUpdateReviewDto, Review>();
        
        CreateMap<Currency, CurrencyDto>();
        CreateMap<Currency, CurrencyLookupDto>();
    }
}