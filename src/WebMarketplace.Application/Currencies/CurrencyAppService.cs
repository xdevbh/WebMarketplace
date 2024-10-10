using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using WebMarketplace.Products;

namespace WebMarketplace.Currencies
{
    [AllowAnonymous]
    public class CurrencyAppService : WebMarketplaceAppService, ICurrencyAppService
    {
        private readonly ICurrencyRepository _currencyRepository;

        public CurrencyAppService(ICurrencyRepository currencyRepository)
        {
            _currencyRepository = currencyRepository;
        }

        public async Task<bool> CurrencyExists(string currency)
        {
            var exists = await _currencyRepository.Exists(currency);
            return exists;
        }

        public async Task<ListResultDto<string>> GetCodeListAsync()
        {
            var codes = await _currencyRepository.GetCodeListAsync();
            return new ListResultDto<string>(codes);
        }

        public async Task<ListResultDto<CurrencyDto>> GetListAsync()
        {
            var currencies = await _currencyRepository.GetListAsync();
            var dtos = ObjectMapper.Map<List<Currency>, List<CurrencyDto>>(currencies);
            return new ListResultDto<CurrencyDto>(dtos);
        }
    }
}
