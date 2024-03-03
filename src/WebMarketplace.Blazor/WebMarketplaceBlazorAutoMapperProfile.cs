using AutoMapper;
using WebMarketplace.UserVendors;

namespace WebMarketplace.Blazor;

public class WebMarketplaceBlazorAutoMapperProfile : Profile
{
    public WebMarketplaceBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<UserVendorDto, CreateUpdateUserVendorDto>();
    }
}