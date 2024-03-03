using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Vendors;

public class VendorAppService : CrudAppService
    <Vendor, VendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateVendorDto>, IVendorAppService
{
    public VendorAppService(IRepository<Vendor, Guid> repository)
        : base(repository)
    {
    }

    public async Task<PagedResultDto<VendorDto>> GetFilteredListAsync(VendorRequestDto input)
    {
        var query = await Repository.GetQueryableAsync();
        
        query = ApplyPaging(query, input);
        query = ApplySorting(query, input);
        query = ApplyFilter(query, input);
        
        var items = await AsyncExecuter.ToListAsync(query);
        var totalCount = items.Count;
        var dtos = ObjectMapper.Map<List<Vendor>, List<VendorDto>>(items);
        
        return new PagedResultDto<VendorDto>(
            totalCount,
            dtos
        );
    }

    public async Task<ListResultDto<VendorDto>> GetAllVendorsAsync()
    {
        var vendors = await Repository.GetListAsync();
        var dtos = ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendors);
        return new ListResultDto<VendorDto>(dtos);
    }

    private IQueryable<Vendor> ApplyFilter(IQueryable<Vendor> query, VendorRequestDto input)
    {
        if (input.Name != null)
        {
            query= query.Where(x=>x.Name.Contains(input.Name));
        }

        return query;
    }
}