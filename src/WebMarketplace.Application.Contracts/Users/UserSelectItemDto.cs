using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Users;

public class UserSelectItemDto : EntityDto<Guid>
{
    public string UserName { get; set; }
}