using AutoMapper;
using WebMarketplace.Products;

namespace WebMarketplace.Blazor.Client;

public class WebMarketplaceBlazorAutoMapperProfile : Profile
{
    public WebMarketplaceBlazorAutoMapperProfile()
    {
        //Define your AutoMapper configuration here for the Blazor project.
        CreateMap<ProductDto, CreateUpdateProductDto>();
    }
}