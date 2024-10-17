using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Users.UserAddresses;

public class UserAddress : CreationAuditedAggregateRoot<Guid>
{
    public virtual Guid UserId { get; private set; }
    public virtual Guid AddressId { get; private set; }

    protected UserAddress()
    {
    }

    public UserAddress(Guid id, Guid userId, Guid addressId)
        : base(id)
    {
        UserId = userId;
        AddressId = addressId;
    }
}