using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies;

public interface ICompanyRepository : IRepository<Company, Guid>
{
    Task<IQueryable<CompanyDetailQueryResultItem>> GetDetailQueryableAsync(
        Guid? companyId = null);

    Task<CompanyDetailQueryResultItem> GetProductDetailAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<CompanyDetailQueryResultItem>> GetDetailListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        CancellationToken cancellationToken = default);

    Task<long> GetDetailCountAsync(
        Guid? companyId = null,
        CancellationToken cancellationToken = default);

    Task<IQueryable<CompanyBlogPost>> GetBlogPostQueryableAsync(
        Guid? companyId = null);

    Task<CompanyBlogPost> GetBlogPostAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<CompanyBlogPost>> GetBlogPostListAsync(
        string? sorting = null,
        int maxResultCount = int.MaxValue,
        int skipCount = 0,
        Guid? companyId = null,
        CancellationToken cancellationToken = default);

    Task<long> GetBlogPostCountAsync(
        Guid? companyId = null,
        CancellationToken cancellationToken = default);
}