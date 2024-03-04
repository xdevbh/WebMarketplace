using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace WebMarketplace.Vendors;

public class UserVendorManager : DomainService
{
    private readonly IRepository<UserVendor, Guid> _userVendorRepository;
    private readonly IRepository<Vendor?, Guid> _vendorRepository;
    private readonly IRepository<IdentityUser, Guid> _userRepository;
    
    public UserVendorManager(
        IRepository<UserVendor, Guid> userVendorRepository,
        IRepository<Vendor?, Guid> vendorRepository,
        IRepository<IdentityUser, Guid> userRepository)
    {
        _userVendorRepository = userVendorRepository;
        _vendorRepository = vendorRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Vendor?> GetVendorByUserAsync(Guid vendorId)
    {
        var userVendor = await _userVendorRepository.FirstOrDefaultAsync(x => x.UserId == vendorId);
        if (userVendor == null)
        {
            return null;
        }
        var vendor =  await _vendorRepository.GetAsync(userVendor.VendorId);
        return vendor;
    }
    
    public async Task<List<IdentityUser>> GetUsersByVendorAsync(Guid vendorId)
    {
        var userVendors = await _userVendorRepository.GetListAsync(x => x.VendorId == vendorId);
        var users = new List<IdentityUser>();
        foreach (var userVendor in userVendors)
        {
            var user = await _userRepository.GetAsync(userVendor.UserId);
            users.Add(user);
        }
        return users;
    }
}