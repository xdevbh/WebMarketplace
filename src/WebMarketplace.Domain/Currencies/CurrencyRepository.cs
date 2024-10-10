using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarketplace.Currencies;

public class CurrencyRepository : ICurrencyRepository
{
    public bool? IsChangeTrackingEnabled { get; set; }

    public Task<List<Currency>> GetListAsync()
    {
        var list = new List<Currency>();

        list.Add(new Currency
        {
            Name = "US Dollar",
            Code = "USD",
            NumericCode = "840",
            Symbol = "$"
        });

        list.Add(new Currency
        {
            Name = "Euro",
            Code = "EUR",
            NumericCode = "978",
            Symbol = "€"
        });

        list.Add(new Currency
        {
            Name = "British Pound",
            Code = "GBP",
            NumericCode = "826",
            Symbol = "£"
        });

        list.Add(new Currency
        {
            Name = "Czech Koruna",
            Code = "CZK",
            NumericCode = "203",
            Symbol = "Kč"
        });

        return Task.FromResult(list);
    }

    public async Task<bool> Exists(string currency)
    {
        var currencies = await GetListAsync();
        return currencies.Any(x => x.Code.Equals(currency.Trim().ToUpper()));
    }

    public async Task<List<string>> GetCodeListAsync()
    {
        var currencies = await GetListAsync();
        var codes = currencies.Select(c => c.Code).ToList();
        return codes;
    }
}
