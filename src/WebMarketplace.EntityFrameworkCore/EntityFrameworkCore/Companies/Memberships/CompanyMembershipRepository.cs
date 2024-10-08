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
using WebMarketplace.Companies;
using WebMarketplace.Companies.Memberships;

namespace WebMarketplace.EntityFrameworkCore.Companies.Memberships;

public class CompanyMembershipRepository : EfCoreRepository<WebMarketplaceDbContext, CompanyMembership, Guid>,
    ICompanyMembershipRepository
{
    public CompanyMembershipRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(
        dbContextProvider)
    {
    }

    public async Task<IQueryable<CompanyMembershipDetailQueryResultItem>> GetCompanyMembershipDetailQueryableAsync(
        Guid? companyId = null,
        Guid? userId = null)
    {
        var dbContext = await GetDbContextAsync();

        var query = from companyMembership in dbContext.Set<CompanyMembership>()
            join company in dbContext.Set<Company>() on companyMembership.CompanyId equals company.Id
            join user in dbContext.Set<IdentityUser>() on companyMembership.UserId equals user.Id
            select new CompanyMembershipDetailQueryResultItem()
            {
                Id = companyMembership.Id,
                CompanyId = companyMembership.CompanyId,
                CompanyName = company.Name,
                CompanyDisplayName = company.DisplayName,
                UserId = companyMembership.UserId,
                UserName = user.UserName,
                Email = user.Email,
                Name = user.Name,
                Surname = user.Surname,
                CreationTime = companyMembership.CreationTime
            };
        
        if (companyId != null)
        {
            query = query.Where(x => x.CompanyId == companyId);
        }

        if(userId != null)
        {
            query = query.Where(x => x.UserId == userId);
        }

        return query;
    }

    public async Task<CompanyMembershipDetailQueryResultItem> GetCompanyMembershipDetailAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = await GetCompanyMembershipDetailQueryableAsync();
        query.Where(x => x.Id == id);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<CompanyMembershipDetailQueryResultItem> GetCompanyMembershipDetailAsync(
        Guid companyId,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetCompanyMembershipDetailQueryableAsync(companyId, userId);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }


    public async Task<List<CompanyMembershipDetailQueryResultItem>> GetCompanyMembershipDetailListAsync(
        string? sorting = null,
        int maxResultCount = Int32.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        Guid? userId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetCompanyMembershipDetailQueryableAsync(companyId, userId);
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = nameof(CompanyMembershipDetailQueryResultItem.CompanyName);
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var items = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return items;
    }

    public async Task<long> GetCompanyMembershipDetailCountAsync(Guid? companyId, Guid? userId,
        CancellationToken cancellationToken = default)
    {
        var query = await GetCompanyMembershipDetailQueryableAsync(companyId, userId);
        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }
}