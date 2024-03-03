using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;
using WebMarketplace.Vendors;

namespace WebMarketplace.UserVendors;

public class UserVendorAppService : CrudAppService
    <UserVendor, UserVendorDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateUserVendorDto>, IUserVendorAppService
{
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly IIdentityUserRepository _userRepository;
    private readonly IdentityUserAppService _userAppService;

    public UserVendorAppService(
        IRepository<UserVendor, Guid> repository,
        IRepository<Vendor, Guid> vendorRepository,
        IIdentityUserRepository userRepository,
        IdentityUserAppService userAppService)
        : base(repository)
    {
        _vendorRepository = vendorRepository;
        _userRepository = userRepository;
        _userAppService = userAppService;
    }

    public async Task<PagedResultDto<UserVendorDto>> GetFilteredListAsync(UserVendorRequestDto input)
    {
        var queryable = await Repository.GetQueryableAsync();
        queryable = ApplyFilter(queryable, input);
        queryable = ApplySorting(queryable, input);
        queryable = ApplyPaging(queryable, input);

        var items = await AsyncExecuter.ToListAsync(queryable);
        var totalCount = await Repository.GetCountAsync();
        var dtos = ObjectMapper.Map<List<UserVendor>, List<UserVendorDto>>(items);
        foreach (var dto in dtos)
        {
            var vendor = await _vendorRepository.GetAsync(dto.VendorId);
            var user = await _userAppService.GetAsync(dto.UserId);
            dto.Vendor = ObjectMapper.Map<Vendor, VendorDto>(vendor);
            dto.User = user;
        }

        return new PagedResultDto<UserVendorDto>(
            totalCount,
            dtos
        );
    }

    private IQueryable<UserVendor> ApplyFilter(IQueryable<UserVendor> queryable, UserVendorRequestDto input)
    {
        if (input.UserId != null)
        {
            queryable = queryable.Where(x=>x.UserId == input.UserId.Value);
        }
        if (input.VendorId != null)
        {
            queryable = queryable.Where(x=> x.VendorId == input.VendorId.Value);
        }
        
        return queryable;
    }
    
    public override async Task<PagedResultDto<UserVendorDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await Repository.GetQueryableAsync();
        queryable = ApplySorting(queryable, input);
        queryable = ApplyPaging(queryable, input);

        var items = await AsyncExecuter.ToListAsync(queryable);
        var totalCount = await Repository.GetCountAsync();
        var dtos = ObjectMapper.Map<List<UserVendor>, List<UserVendorDto>>(items);
        foreach (var dto in dtos)
        {
            var vendor = await _vendorRepository.GetAsync(dto.VendorId);
            var user = await _userAppService.GetAsync(dto.UserId);
            dto.Vendor = ObjectMapper.Map<Vendor, VendorDto>(vendor);
            dto.User = user;
        }

        return new PagedResultDto<UserVendorDto>(
            totalCount,
            dtos
        );
    }
    
    public override async Task<UserVendorDto> GetAsync(Guid id)
    {
        var userVendor = await base.GetAsync(id);
        var vendor = await _vendorRepository.GetAsync(userVendor.VendorId);
        var user = await _userAppService.GetAsync(userVendor.UserId);
        userVendor.Vendor = ObjectMapper.Map<Vendor, VendorDto>(vendor);
        userVendor.User = user;
        return userVendor;
    }
    
    public async Task<List<IdentityUserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetListAsync();
        return ObjectMapper.Map<List<IdentityUser>, List<IdentityUserDto>>(users);
    }
    
    public async Task<List<VendorDto>> GetAllVendorsAsync()
    {
        var vendors = await _vendorRepository.GetListAsync();
        return ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendors);
    }

    public override Task<UserVendorDto> UpdateAsync(Guid id, CreateUpdateUserVendorDto input)
    {
        return base.UpdateAsync(id, input);
    }
}