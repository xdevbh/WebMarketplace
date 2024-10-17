using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Addresses;

public interface IAddressAppService: IApplicationService
{
    Task<PagedResultDto<AddressDto>> GetListAsync(AddressFilterDto input);
    Task<AddressDto> GetAsync(Guid id);
    Task<AddressDto> UpdateAsync(Guid id, CreateUpdateAddressDto input);
    Task<AddressDto> CreateAsync(CreateUpdateAddressDto input);
    Task DeleteAsync(Guid id);
}