using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;
using WebMarketplace.Addresses;
using WebMarketplace.Products;
using WebMarketplace.Companies;
using WebMarketplace.Companies.Memberships;
using WebMarketplace.Currencies;
using WebMarketplace.Orders;
using WebMarketplace.Users.UserAddresses;

namespace WebMarketplace;

public class WebMarketplaceApplicationAutoMapperProfile : Profile
{
    public WebMarketplaceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Currency, CurrencyDto>();

        CreateMap<Company, CompanyDto>();
        CreateMap<Company, CompanyLookupDto>();
        CreateMap<CompanyMembershipDetailQueryResultItem, CompanyMembershipDto>();
        CreateMap<CreateCompanyMembershipDto, CompanyMembership>();
        CreateMap<CompanyDto, CreateUpdateCompanyAdminDto>();
        CreateMap<CompanyImage, CompanyImageDto>()
            .Ignore(x => x.Content);

        CreateMap<Address, AddressDto>();
        CreateMap<CreateUpdateAddressDto, Address>()
            .Ignore(x => x.Id);

        CreateMap<Product, ProductDto>();
        CreateMap<ProductDetailQueryRequestItem, ProductDto>();
        CreateMap<ProductDetailQueryRequestItem, ProductDetailDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<ProductDetailQueryRequestItem, ProductCardDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency)
            .Ignore(x => x.ImageContent)
            .Ignore(x => x.ImageContentType);
        CreateMap<ProductDetailQueryRequestItem, ProductListItemDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<ProductDto, CreateUpdateProductDto>();


        CreateMap<ProductReviewDetailQueryResultItem, ProductReviewDto>();
        CreateMap<CreateUpdateProductReviewDto, ProductReviewDto>();

        CreateMap<ProductImage, ProductImageDto>()
            .Ignore(x => x.Content);
        CreateMap<ProductPrice, ProductPriceDto>();

        CreateMap<Order, OrderDto>()
            .Ignore(x => x.Buyer)
            .Ignore(x => x.ShippingAddress)
            .Ignore(x => x.Items);
        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Order, OrderCardDto>();
        CreateMap<Order, OrderListItemDto>()
            .Ignore(x => x.Buyer);
        CreateMap<Buyer, BuyerDto>();
        CreateMap<ShippingAddress, ShippingAddressDto>();

        CreateMap<UserAddressDetailQueryResultItem, UserAddressDto>();
    }
}