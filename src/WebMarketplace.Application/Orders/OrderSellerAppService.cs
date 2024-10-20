using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Users;
using WebMarketplace.Companies.Memberships;

namespace WebMarketplace.Orders;

public class OrderSellerAppService: WebMarketplaceAppService, IOrderSellerAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderManager _orderManager;
    private readonly ICompanyMembershipRepository _companyMembershipRepository;

    public OrderSellerAppService(IOrderRepository orderRepository, OrderManager orderManager, ICompanyMembershipRepository companyMembershipRepository)
    {
        _orderRepository = orderRepository;
        _orderManager = orderManager;
        _companyMembershipRepository = companyMembershipRepository;
    }

    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        var companyId = await GetCompanyId();
        
        if (order == null || order.CompanyId != companyId)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderNotFound).WithData("Id", id);
        }

        var dto = await MapOrderAsync(order);
        return dto;
    }

    public async Task<PagedResultDto<OrderListItemDto>> GetListAsync(OrderFilterSellerDto input)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        var companyId = await GetCompanyId();

        var totalCount = await _orderRepository.GetFilteredCountAsync(
            companyId,
            userId,
            input.Status
        );

        var orders = await _orderRepository.GetFilteredListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            companyId,
            userId,
            input.Status
        );

        var dtos = new List<OrderListItemDto>();
        foreach (var order in orders)
        {
            var dto = await MapOrderListItemAsync(order);
            dtos.Add(dto);
        }
        
        return new PagedResultDto<OrderListItemDto>(totalCount, dtos);
    }

    public async Task<OrderDto> ChangeStatusAsync(ChangeStatusDto input)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }
        
        var companyId = await GetCompanyId();
        
        var order = await _orderRepository.GetAsync(input.OrderId);
        if (order == null || order.CompanyId != companyId)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderNotFound).WithData("Id", input.OrderId);
        }
        
        await _orderManager.ChangeStatusAsync(order, input.Status);
        var dto = await MapOrderAsync(order);
        return dto;
    }
    

    #region Helpers

    private async Task<OrderDto> MapOrderAsync(Order order)
    {
        var dto = ObjectMapper.Map<Order, OrderDto>(order);
        dto.Items = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(order.Items);
        dto.Buyer = ObjectMapper.Map<Buyer, BuyerDto>(order.Buyer);
        dto.ShippingAddress = ObjectMapper.Map<ShippingAddress, ShippingAddressDto>(order.ShippingAddress);
        return dto;
    }
    
    private async Task<OrderListItemDto> MapOrderListItemAsync(Order order)
    {
        var dto = ObjectMapper.Map<Order, OrderListItemDto>(order);
        dto.Buyer = ObjectMapper.Map<Buyer, BuyerDto>(order.Buyer);
        return dto;
    }
    
    private async Task<Guid> GetCompanyId()
    {
        var userId = CurrentUser.GetId();
        var companyMembership = await _companyMembershipRepository.GetAsync(x => x.UserId == userId);
        if (companyMembership == null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.UserNotCompanyMember);
        }

        return companyMembership.CompanyId;
    }

    #endregion
}