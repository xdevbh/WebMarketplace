using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace WebMarketplace.Users.UserAddresses;

public class UserAddressManager: DomainService
{
    private readonly IUserAddressRepository _userAddressRepository;

    public UserAddressManager(IUserAddressRepository userAddressRepository)
    {
        _userAddressRepository = userAddressRepository;
    }
    
    public async Task AddAsync(
        Guid userId,
        Guid addressId)
    {
        if (await IsAddedAsync(userId, addressId))
        {
            return;
        }

        var userAddress = await _userAddressRepository.InsertAsync(
            new UserAddress(
                GuidGenerator.Create(),
                userId,
                addressId
            )
        );
    }
    
    public async Task<bool> IsAddedAsync(
        Guid userId,
        Guid addressId)
    {
        return await _userAddressRepository
            .AnyAsync(x => x.UserId == userId && x.AddressId == addressId);
    }
}