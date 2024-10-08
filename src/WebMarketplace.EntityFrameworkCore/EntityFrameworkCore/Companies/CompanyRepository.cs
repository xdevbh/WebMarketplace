using System;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMarketplace.Companies;

namespace WebMarketplace.EntityFrameworkCore.Companies;

public class CompanyRepository: EfCoreRepository<WebMarketplaceDbContext, Company, Guid>,
    ICompanyRepository
{
    public CompanyRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }
}