using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Users.UserAddresses;

public interface IUserAddressRepository : IRepository<UserAddress, Guid>
{
    Task<IQueryable<UserAddressDetailQueryResultItem>>  GetDetailQueryableAsync(
        Guid? userId = null,
        Guid? addressId = null);
    
    Task<UserAddressDetailQueryResultItem> GetDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<UserAddressDetailQueryResultItem> GetDetailAsync(
        Guid userId,
        Guid addressId,
        CancellationToken cancellationToken = default);
    
    Task<List<UserAddressDetailQueryResultItem>> GetDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? userId= null,
        Guid? addressId= null,
        CancellationToken cancellationToken = default);
    
    Task<long> GetDetailCountAsync(
        Guid? userId= null,
        Guid? addressId= null,
        CancellationToken cancellationToken = default);
}