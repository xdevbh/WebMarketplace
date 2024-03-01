using System;
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
}