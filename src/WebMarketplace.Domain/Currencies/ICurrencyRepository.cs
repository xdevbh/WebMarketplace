using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Currencies;

public interface ICurrencyRepository : IRepository
{
    Task<List<Currency>> GetListAsync();
    Task<List<string>> GetCodeListAsync();
    Task<bool> Exists(string currency);
}