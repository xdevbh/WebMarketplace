using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Permissions;

namespace WebMarketplace.Addresses;

[Authorize(WebMarketplacePermissions.Addresses.Default)]
public class AddressAppService : WebMarketplaceAppService, IAddressAppService
{
    private readonly IRepository<Address, Guid> _addressRepository;

    public AddressAppService(IRepository<Address, Guid> addressRepository)
    {
        _addressRepository = addressRepository;
    }

    public async Task<PagedResultDto<AddressDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var addresses = await _addressRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _addressRepository.GetCountAsync();
        
        return new PagedResultDto<AddressDto>(
            totalCount,
            ObjectMapper.Map<List<Address>, List<AddressDto>>(addresses)
        );
    }

    public async Task<AddressDto> GetAsync(Guid id)
    {
        var address = await _addressRepository.GetAsync(id);
        var addressDto = ObjectMapper.Map<Address, AddressDto>(address);
        return addressDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Update)]
    public async Task<AddressDto> UpdateAsync(Guid id, CreateUpdateAddressDto input)
    {
        var address = await _addressRepository.GetAsync(id);
        address = ObjectMapper.Map<CreateUpdateAddressDto, Address>(input);
        await _addressRepository.UpdateAsync(address);
        
        var addressDto = ObjectMapper.Map<Address, AddressDto>(address);
        return addressDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Create)]
    public async Task<AddressDto> CreateAsync(CreateUpdateAddressDto input)
    {
        var address = ObjectMapper.Map<CreateUpdateAddressDto, Address>(input);
        await _addressRepository.InsertAsync(address);
        
        var addressDto = ObjectMapper.Map<Address, AddressDto>(address);
        return addressDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Delete)]
    public async  Task DeleteAsync(Guid id)
    {
        await _addressRepository.DeleteAsync(id);
    }
}