using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Users;

public interface IUserAdminAppService : IApplicationService
{
    Task<ListResultDto<UserSelectItemDto>> GetSelectItemListAsync();
}