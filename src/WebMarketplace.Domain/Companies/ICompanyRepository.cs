using System;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Companies;

public interface ICompanyRepository : IRepository<Company, Guid>
{
    
}