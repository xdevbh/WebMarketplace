using AutoMapper;
using WebMarketplace.Addresses;
using WebMarketplace.Products;
using WebMarketplace.Vendors;

namespace WebMarketplace;

public class WebMarketplaceApplicationAutoMapperProfile : Profile
{
    public WebMarketplaceApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        
        CreateMap<Vendor, VendorDto>();

        CreateMap<Address, AddressDto>();
        CreateMap<CreateUpdateAddressDto, Address>();
        
        CreateMap<Product, ProductDto>();
    }
}
