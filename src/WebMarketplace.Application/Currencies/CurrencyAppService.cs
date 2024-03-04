using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Products;

namespace WebMarketplace.Currencies;

public class CurrencyAppService : WebMarketplaceAppService, ICurrencyAppService
{
    public IRepository<Currency, Guid> _currencyRepository { get; set; }

    public CurrencyAppService(IRepository<Currency, Guid> currencyRepository)
    {
        _currencyRepository = currencyRepository;
    }

    public async Task<CurrencyDto> GetAsync(Guid id)
    {
        var c = await _currencyRepository.GetAsync(id);
        return ObjectMapper.Map<Currency, CurrencyDto>(c);
    }

    public async Task<ListResultDto<CurrencyDto>> GetListAsync()
    {
        var currencies = await _currencyRepository.GetListAsync();
        var dtos = ObjectMapper.Map<List<Currency>, List<CurrencyDto>>(currencies);
        return new ListResultDto<CurrencyDto>(dtos);
    }
    
    public async Task<CurrencyDto> GetByCodeAsync(string code)
    {
        var c = await _currencyRepository.GetAsync(x => x.Code == code);
        return ObjectMapper.Map<Currency, CurrencyDto>(c);
    }

    public async Task<ListResultDto<CurrencyLookupDto>> GetLookupListAsync()
    {
        var currencies = await _currencyRepository.GetListAsync();
        var dtos = ObjectMapper.Map<List<Currency>, List<CurrencyLookupDto>>(currencies);
        return new ListResultDto<CurrencyLookupDto>(dtos);
    }
    
    
}