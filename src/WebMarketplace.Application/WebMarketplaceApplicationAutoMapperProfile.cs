using AutoMapper;
using WebMarketplace.Carts;
using WebMarketplace.Orders;
using WebMarketplace.ProductCategories;
using WebMarketplace.Products;
using WebMarketplace.Reviews;
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
        CreateMap<Product, CreateUpdateProductDto>();
        
        CreateMap<ProductCategory, ProductCategoryDto>();
        CreateMap<ProductCategory, CreateUpdateProductCategoryDto>();
        
        CreateMap<Cart, CartDto>();
        CreateMap<Cart, CreateUpdateCartDto>();
        
        CreateMap<CartItem, CartItemDto>();
        CreateMap<CartItem, CreateUpdateCartItemDto>();
        
        CreateMap<Order, OrderDto>();
        CreateMap<Order, CreateUpdateOrderDto>();
        
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<OrderItem, CreateUpdateOrderItemDto>();
        
        CreateMap<Vendor, VendorDto>();
        CreateMap<Vendor, CreateUpdateVendorDto>();
        
        CreateMap<UserVendor, UserVendorDto>();
        CreateMap<UserVendor, CreateUpdateVendorDto>();
        
        CreateMap<Review, ReviewDto>();
        CreateMap<Review, CreateUpdateReviewDto>();
    }
}