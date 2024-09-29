using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebMarketplace.Currencies;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetListAsync();
}