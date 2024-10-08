using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace WebMarketplace.Users;

[Authorize("AdminOnly")]
public class UserAdminAppService : WebMarketplaceAppService, IUserAdminAppService
{
    private readonly IRepository<IdentityUser, Guid> _userRepository;

    public UserAdminAppService(IRepository<IdentityUser, Guid> userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ListResultDto<UserSelectItemDto>> GetSelectItemListAsync()
    {
        var users = await _userRepository.GetListAsync();
        var dtos = users.Select(u => new UserSelectItemDto
        {
            Id = u.Id,
            UserName = u.UserName
        }).ToList();

        return new ListResultDto<UserSelectItemDto>(dtos);
    }
}