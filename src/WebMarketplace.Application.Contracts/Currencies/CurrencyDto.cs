using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Currencies;

public class CurrencyDto : EntityDto<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string NumericCode { get; set; }
    public string Symbol { get; set; }
}