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
    private readonly IRepository<Address, Guid> _vendorRepository;

    public AddressAppService(IRepository<Address, Guid> vendorRepository)
    {
        _vendorRepository = vendorRepository;
    }

    public async Task<PagedResultDto<AddressDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var vendorList = await _vendorRepository.GetPagedListAsync(input.SkipCount, input.MaxResultCount, input.Sorting);
        var totalCount = await _vendorRepository.GetCountAsync();
        
        return new PagedResultDto<AddressDto>(
            totalCount,
            ObjectMapper.Map<List<Address>, List<AddressDto>>(vendorList)
        );
    }

    public async Task<AddressDto> GetAsync(Guid id)
    {
        var vendor = await _vendorRepository.GetAsync(id);
        var vendorDto = ObjectMapper.Map<Address, AddressDto>(vendor);
        return vendorDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Update)]
    public async Task<AddressDto> UpdateAsync(Guid id, CreateUpdateAddressDto input)
    {
        var address = await _vendorRepository.GetAsync(id);
        address = ObjectMapper.Map<CreateUpdateAddressDto, Address>(input);
        await _vendorRepository.UpdateAsync(address);
        
        var addressDto = ObjectMapper.Map<Address, AddressDto>(address);
        return addressDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Create)]
    public async Task<AddressDto> CreateAsync(CreateUpdateAddressDto input)
    {
        var address = ObjectMapper.Map<CreateUpdateAddressDto, Address>(input);
        await _vendorRepository.InsertAsync(address);
        
        var addressDto = ObjectMapper.Map<Address, AddressDto>(address);
        return addressDto;
    }

    [Authorize(WebMarketplacePermissions.Addresses.Delete)]
    public async  Task DeleteAsync(Guid id)
    {
        await _vendorRepository.DeleteAsync(id);
    }
}