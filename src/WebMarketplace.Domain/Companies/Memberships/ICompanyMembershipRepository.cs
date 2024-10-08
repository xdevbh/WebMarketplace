using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies.Memberships;

public interface ICompanyMembershipRepository : IRepository<CompanyMembership, Guid>
{
    Task<IQueryable<CompanyMembershipDetailQueryResultItem>> GetCompanyMembershipDetailQueryableAsync(
        Guid? companyId = null,
        Guid? userId = null);
    
    Task<CompanyMembershipDetailQueryResultItem> GetCompanyMembershipDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default);
    
    Task<CompanyMembershipDetailQueryResultItem> GetCompanyMembershipDetailAsync(
        Guid companyId,
        Guid userId,
        CancellationToken cancellationToken = default);
    
    Task<List<CompanyMembershipDetailQueryResultItem>> GetCompanyMembershipDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId= null,
        Guid? userId= null,
        CancellationToken cancellationToken = default);

    Task<long> GetCompanyMembershipDetailCountAsync(
        Guid? companyId,
        Guid? userId,
        CancellationToken cancellationToken = default);

}