using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Internal.Mappers;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AutoMapper;
using WebMarketplace.Addresses;
using WebMarketplace.Products;
using WebMarketplace.Companies;

namespace WebMarketplace;

public class WebMarketplaceApplicationAutoMapperProfile : Profile
{
    public WebMarketplaceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Company, CompanyDto>();

        CreateMap<Address, AddressDto>();
        CreateMap<CreateUpdateAddressDto, Address>();

        CreateMap<Product, ProductDto>()
            .Ignore(x => x.CompanyName);
        CreateMap<Product, ProductDetailDto>()
            .Ignore(x => x.CompanyName)
            .ForMember(dest => dest.PriceCurrency,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.Currency : string.Empty))
            .ForMember(dest => dest.PriceAmount,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.Amount : 0));
        ;
        CreateMap<CreateUpdateProductDto, Product>();
        CreateMap<Product, ProductCardDto>()
            .ForMember(dest => dest.Rating,
                opt => opt.MapFrom(src => src.Rating))
            .ForMember(dest => dest.PriceCurrency,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.Currency : string.Empty))
            .ForMember(dest => dest.PriceAmount,
                opt => opt.MapFrom(src => src.ProductPrice != null ? src.ProductPrice.Amount : 0));

        CreateMap<ProductReview, ProductReviewDto>()
            .Ignore(x => x.UserName);
        CreateMap<CreateUpdateProductReviewDto, ProductReviewDto>();
    }
}