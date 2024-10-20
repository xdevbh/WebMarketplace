using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Polly.Retry;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Users;
using WebMarketplace.Addresses;

namespace WebMarketplace.Users.UserAddresses;

public class UserAddressAppService : WebMarketplaceAppService, IUserAddressAppService
{
    private readonly IUserAddressRepository _userAddressRepository;
    private readonly IRepository<Address, Guid> _addressRepository;
    private readonly UserAddressManager _userAddressManager;

    public UserAddressAppService(
        IUserAddressRepository userAddressRepository,
        IRepository<Address, Guid> addressRepository, 
        UserAddressManager userAddressManager)
    {
        _userAddressRepository = userAddressRepository;
        _addressRepository = addressRepository;
        _userAddressManager = userAddressManager;
    }

    public async Task<UserAddressDto> GetAsync(Guid id)
    {
        var item = await _userAddressRepository.GetDetailAsync(id);
        var dto = ObjectMapper.Map<UserAddressDetailQueryResultItem, UserAddressDto>(item);
        return dto;
    }

    public async Task<PagedResultDto<UserAddressDto>> GetListAsync(UserAddressFilterDto input)
    {
        var totalCount = await _userAddressRepository.GetDetailCountAsync(
            input.UserId,
            input.AddressId);

        var items = await _userAddressRepository.GetDetailListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            input.UserId,
            input.AddressId
        );

        var dtos = ObjectMapper.Map<List<UserAddressDetailQueryResultItem>, List<UserAddressDto>>(items);
        return new PagedResultDto<UserAddressDto>(totalCount, dtos);
    }
    
    public async Task<PagedResultDto<UserAddressDto>> GetMyListAsync(PagedAndSortedResultRequestDto input)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        var totalCount = await _userAddressRepository.GetDetailCountAsync(userId);

        var items = await _userAddressRepository.GetDetailListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            userId
        );

        var dtos = ObjectMapper.Map<List<UserAddressDetailQueryResultItem>, List<UserAddressDto>>(items);
        return new PagedResultDto<UserAddressDto>(totalCount, dtos);
    }

    public async Task<ListResultDto<UserAddressSelectItemDto>> GetMySelectItemListListAsync()
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var totalCount = await _userAddressRepository.GetDetailCountAsync(userId);

        var query = await _userAddressRepository.GetDetailQueryableAsync(userId);
        var items = await AsyncExecuter.ToListAsync(query);
        var dtos = items.Select(x => new UserAddressSelectItemDto()
        {
            Id = x.AddressId,
            Text = $"{x.FullName}, {x.Line1}, {x.City}, {x.Country}, {x.ZipCode}"
        }).ToList();

        return new ListResultDto<UserAddressSelectItemDto>(dtos);
    }

    public async Task<ListResultDto<UserAddressSelectItemDto>> GetSelectItemListListAsync(Guid userId)
    {
        var totalCount = await _userAddressRepository.GetDetailCountAsync(userId);

        var query = await _userAddressRepository.GetDetailQueryableAsync(userId);
        var items = await AsyncExecuter.ToListAsync(query);
        var dtos = items.Select(x => new UserAddressSelectItemDto()
        {
            Id = x.AddressId,
            Text = $"{x.FullName}, {x.Line1}, {x.City}, {x.Country}, {x.ZipCode}"
        }).ToList();

        return new ListResultDto<UserAddressSelectItemDto>(dtos);
    }

    public async Task<UserAddressDto> CreateAsync(CreateUserAddressDto input)
    {
        await _userAddressManager.AddAsync(input.AddressId, input.UserId);
        var item = await _userAddressRepository.GetDetailAsync(input.AddressId, input.UserId);
        var dto = ObjectMapper.Map<UserAddressDetailQueryResultItem, UserAddressDto>(item);
        return dto;
    }

    public async Task<UserAddressDto> CreateMyAsync(Guid addressId)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var address = await _addressRepository.GetAsync(addressId);
        if (address == null)
        {
            throw new BusinessException(L["AddressNotFound"]);
        }

        await _userAddressManager.AddAsync(userId, addressId);
        var item = await _userAddressRepository.GetDetailAsync(userId, addressId);
        var dto = ObjectMapper.Map<UserAddressDetailQueryResultItem, UserAddressDto>(item);
        return dto;
    }


    public async Task DeleteAsync(Guid id)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        var item = await _userAddressRepository.GetAsync(id);
        
        if (item == null || item.UserId != userId)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.AddressNotFound);
        }
        
        await _addressRepository.DeleteAsync(item.AddressId);
        await _userAddressRepository.DeleteAsync(id);
    }
}