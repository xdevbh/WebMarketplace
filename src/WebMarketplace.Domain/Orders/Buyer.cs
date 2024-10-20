using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Values;

namespace WebMarketplace.Orders;

public class Buyer : ValueObject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    protected Buyer()
    {
    }

    public Buyer(Guid id, string name, string email)
    {
        Name = name;
        Email = email;
        Id = id;
    }
    
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Id;
        yield return Name;
        yield return Email;
    }
}