using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Users.UserAddresses;

public interface IUserAddressAppService :IApplicationService
{
    Task<UserAddressDto> GetAsync(Guid id);
    
    Task<PagedResultDto<UserAddressDto>> GetListAsync(UserAddressFilterDto input);
    
    Task<ListResultDto<UserAddressSelectItemDto>> GetMySelectItemListListAsync();
    
    Task<ListResultDto<UserAddressSelectItemDto>> GetSelectItemListListAsync(Guid userId);
    
    Task<UserAddressDto> CreateAsync(CreateUserAddressDto input);
    Task<UserAddressDto> CreateMyAsync(Guid addressId);
    
    Task DeleteAsync(Guid id);
}