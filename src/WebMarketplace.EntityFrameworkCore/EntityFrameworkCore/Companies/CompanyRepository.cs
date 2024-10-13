using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using WebMarketplace.Companies;
using WebMarketplace.Products;

namespace WebMarketplace.EntityFrameworkCore.Companies;

public class CompanyRepository: EfCoreRepository<WebMarketplaceDbContext, Company, Guid>,
    ICompanyRepository
{
    public CompanyRepository(IDbContextProvider<WebMarketplaceDbContext> dbContextProvider) : base(dbContextProvider)
    {
    }

    public override async Task<IQueryable<Company>> WithDetailsAsync()
    {
        return (await GetQueryableAsync()).IncludeDetails();
    }
}