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

namespace WebMarketplace;

public class WebMarketplaceApplicationAutoMapperProfile : Profile
{
    public WebMarketplaceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Company, CompanyDto>();
        CreateMap<Company, CompanyLookupDto>();
        CreateMap<CompanyMembershipDetailQueryResultItem, CompanyMembershipDto>();
        CreateMap<CreateCompanyMembershipDto, CompanyMembership>();
        CreateMap<CompanyDto, CreateUpdateCompanyAdminDto>();

        CreateMap<Address, AddressDto>();
        CreateMap<CreateUpdateAddressDto, Address>();

        CreateMap<Product, ProductDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency)
            .Ignore(x => x.Rating);
        CreateMap<ProductDetailQueryRequestItem, ProductDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<ProductDetailQueryRequestItem, ProductDetailDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<ProductDetailQueryRequestItem, ProductCardDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<ProductDetailQueryRequestItem, ProductListItemDto>()
            .Ignore(x => x.PriceAmount)
            .Ignore(x => x.PriceCurrency);
        CreateMap<CreateUpdateProductDto, Product>();


        CreateMap<ProductReviewDetailQueryResultItem, ProductReviewDto>()
            .Ignore(x => x.UserName);
        CreateMap<CreateUpdateProductReviewDto, ProductReviewDto>();

        CreateMap<ProductImage, ProductImageDto>()
            .Ignore(x => x.Content);
    }
}