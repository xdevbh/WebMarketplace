using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using WebMarketplace.Addresses;
using WebMarketplace.Users.UserAddresses;

namespace WebMarketplace.EntityFrameworkCore.Users.UserAddresses;

public class UserAddressRepository : EfCoreRepository<WebMarketplaceDbContext, UserAddress, Guid>,
    IUserAddressRepository
{
    public UserAddressRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<IQueryable<UserAddressDetailQueryResultItem>> GetDetailQueryableAsync(
        Guid? userId = null,
        Guid? addressId = null)
    {
        var dbContext = await GetDbContextAsync();

        var query = from userAddress in dbContext.Set<UserAddress>()
            join address in dbContext.Set<Address>() on userAddress.AddressId equals address.Id
            join user in dbContext.Set<IdentityUser>() on userAddress.UserId equals user.Id
            select new UserAddressDetailQueryResultItem()
            {
                Id = userAddress.Id,
                UserId = userAddress.UserId,
                AddressId = userAddress.AddressId,
                FullName = address.FullName,
                Country = address.Country,
                State = address.State,
                City = address.City,
                Line1 = address.Line1,
                Line2 = address.Line2,
                ZipCode = address.ZipCode,
                Note = address.Note,
                PhoneNumber = address.PhoneNumber,
                Email = user.Email,
                CreationTime = userAddress.CreationTime,
            };

        if (userId != null)
        {
            query = query.Where(x => x.UserId == userId);
        }

        if (addressId != null)
        {
            query = query.Where(x => x.AddressId == addressId);
        }

        return query;
    }

    public async Task<UserAddressDetailQueryResultItem> GetDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync();
        query.Where(x => x.Id == id);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<UserAddressDetailQueryResultItem> GetDetailAsync(
        Guid userId,
        Guid addressId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync(userId, addressId);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<List<UserAddressDetailQueryResultItem>> GetDetailListAsync(
        string? sorting = null,
        int maxResultCount = Int32.MaxValue,
        int skipCount = 0,
        Guid? userId = null,
        Guid? addressId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync(userId, addressId);
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = "CreationTime DESC";
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var items = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return items;
    }

    public async Task<long> GetDetailCountAsync(
        Guid? userId = null,
        Guid? addressId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync(userId, addressId);
        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }
}