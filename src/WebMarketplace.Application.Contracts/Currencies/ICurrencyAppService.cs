using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Currencies
{
    public interface ICurrencyAppService : IApplicationService
    {
        Task<ListResultDto<CurrencyDto>> GetListAsync();

        Task<ListResultDto<string>> GetCodeListAsync();

        Task<bool> CurrencyExists(string currency);
    }
}
