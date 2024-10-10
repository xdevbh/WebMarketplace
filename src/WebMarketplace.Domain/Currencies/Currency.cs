using System;
using Volo.Abp.Domain.Entities;

namespace WebMarketplace.Currencies;

public class Currency : Entity
{
    public string Name { get; set; }

    public string Code { get; set; }

    public string NumericCode { get; set; }

    public string Symbol { get; set; }


    public override object?[] GetKeys()
    {
        return new object?[] { Code };
    }
}