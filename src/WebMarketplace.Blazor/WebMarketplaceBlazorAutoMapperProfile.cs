using AutoMapper;
using WebMarketplace.Products;
using WebMarketplace.UserVendors;
using WebMarketplace.Vendors;

namespace WebMarketplace.Blazor;

public class WebMarketplaceBlazorAutoMapperProfile : Profile
{
    public WebMarketplaceBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<UserVendorDto, CreateUpdateUserVendorDto>();
        CreateMap<VendorDto, CreateUpdateVendorDto>();
        CreateMap<ProductDto, CreateUpdateProductDto>();


    }
}