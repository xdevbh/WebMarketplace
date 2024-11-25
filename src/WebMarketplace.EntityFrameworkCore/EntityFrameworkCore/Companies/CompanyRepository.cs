using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMarketplace.Addresses;
using WebMarketplace.Companies;
using WebMarketplace.Products;

namespace WebMarketplace.EntityFrameworkCore.Companies;

public class CompanyRepository : EfCoreRepository<WebMarketplaceDbContext, Company, Guid>,
    ICompanyRepository
{
    public CompanyRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Company>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }

    public async Task<IQueryable<CompanyDetailQueryResultItem>> GetDetailQueryableAsync(
        Guid? companyId = null)
    {
        var dbContext = await GetDbContextAsync();
        var query =
            from company in dbContext.Set<Company>().IncludeDetails()
            join address in dbContext.Set<Address>() on company.AddressId equals address.Id
            select new CompanyDetailQueryResultItem
            {
                Id = company.Id,
                CompanyIdentificationNumber = company.CompanyIdentificationNumber,
                Name = company.Name,
                DisplayName = company.DisplayName,
                ShortDescription = company.ShortDescription,
                FullDescription = company.FullDescription,
                Website = company.Website,
                AddressId = company.AddressId,
                Country = address.Country,
                State = address.State,
                City = address.City,
                Line1 = address.Line1,
                Line2 = address.Line2,
                ZipCode = address.ZipCode,
                DefaultImageBlobName = company.Images.Any()
                    ? company.Images.FirstOrDefault(x => x.IsDefault) != null
                        ? company.Images.FirstOrDefault(x => x.IsDefault).BlobName
                        : company.Images.FirstOrDefault().BlobName
                    : string.Empty,
                CreationTime = company.CreationTime
            };

        return query;
    }
    
    public async Task<CompanyDetailQueryResultItem> GetProductDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync();
        query = query.Where(x => x.Id == id);
        var item = await query.FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        return item;
    }

    public async Task<List<CompanyDetailQueryResultItem>> GetDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync(companyId);
        if (sorting.IsNullOrWhiteSpace())
        {
            sorting = "CreationTime DESC";
        }

        query = query.OrderBy(sorting);
        query = query.PageBy(skipCount, maxResultCount);
        var list = await query.ToListAsync(GetCancellationToken(cancellationToken));
        return list;
    }

    public async Task<long> GetDetailCountAsync(
        Guid? companyId = null,
        CancellationToken cancellationToken = default)
    {
        var query = await GetDetailQueryableAsync(companyId);
        
        var count = await query.LongCountAsync(GetCancellationToken(cancellationToken));
        return count;
    }
}