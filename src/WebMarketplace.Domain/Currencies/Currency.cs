using System;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Currencies;

public class Currency : AggregateRoot<Guid>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public string NumericCode { get; set; }
    public string Symbol { get; set; }
    // public string Country { get; set; }
}