using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using WebMarketplace.Common;
using WebMarketplace.Products;

namespace WebMarketplace.Currencies;

public interface ICurrencyAppService : IApplicationService
{
    Task<CurrencyDto> GetAsync(Guid id);

    Task<ListResultDto<CurrencyDto>> GetListAsync();

    Task<CurrencyDto> GetByCodeAsync(string code);
    
    Task<ListResultDto<CurrencyLookupDto>> GetLookupListAsync();
    Task<ListResultDto<SelectOptionDto>> GetSelectOptionListAsync();

}