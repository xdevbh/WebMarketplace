using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;
using Volo.Abp.Users;
using WebMarketplace.Addresses;

namespace WebMarketplace.Orders;

[Authorize]
public class OrderBuyerAppService : WebMarketplaceAppService, IOrderBuyerAppService
{
    private readonly IOrderRepository _orderRepository;
    private readonly OrderManager _orderManager;

    public OrderBuyerAppService(IOrderRepository orderRepository, OrderManager orderManager)
    {
        _orderRepository = orderRepository;
        _orderManager = orderManager;
    }

    public async Task<OrderDto> GetAsync(Guid id)
    {
        var order = await _orderRepository.GetAsync(id);
        if (order == null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderNotFound).WithData("Id", id);
        }

        var dto = await MapAsync(order);
        return dto;
    }

    public async Task<PagedResultDto<OrderCardDto>> GetListAsync(OrderBuyerFilterDto input)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var totalCount = await _orderRepository.GetFilteredCountAsync(
            null,
            userId,
            input.Status
        );

        var orders = await _orderRepository.GetFilteredListAsync(
            input.Sorting,
            input.MaxResultCount,
            input.SkipCount,
            null,
            userId,
            input.Status
        );

        var dtos = ObjectMapper.Map<List<Order>, List<OrderCardDto>>(orders);
        return new PagedResultDto<OrderCardDto>(totalCount, dtos);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderBuyerDto input)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var order = await _orderManager.CreateAsync(
            userId,
            input.AddressId,
            input.CompanyId,
            input.CompanyName,
            input.Items.Select(x => (x.ProductId, x.ProductName, x.Quantity, x.UnitPrice, x.Currency)).ToList()
        );

        var dto =  await MapAsync(order);
        return dto;
    }

    public async Task<OrderDto> CancelAsync(Guid id)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var order = await _orderRepository.GetAsync(id);
        if (order == null || order.Buyer.Id != userId)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderNotFound).WithData("Id", id);
        }

        await _orderManager.ChangeStatusAsync(order.Id, OrderStatus.Cancelled);

        var dto =  await MapAsync(order);
        return dto;
    }

    public async Task<bool> HasPurchasedProductAsync(Guid id)
    {
        var userId = CurrentUser.GetId();
        if (userId == Guid.Empty)
        {
            throw new AbpAuthorizationException(
                L[WebMarketplaceDomainErrorCodes.UserNotAuthenticated],
                WebMarketplaceDomainErrorCodes.UserNotAuthenticated);
        }

        var products = await _orderRepository.GetOrderedProductIdListAsync(userId);
        return products.Contains(id);
    }

    private async Task<OrderDto> MapAsync(Order order)
    {
        var dto = ObjectMapper.Map<Order, OrderDto>(order);
        dto.Items = ObjectMapper.Map<List<OrderItem>, List<OrderItemDto>>(order.Items);
        dto.Buyer = ObjectMapper.Map<Buyer, BuyerDto>(order.Buyer);
        dto.ShippingAddress = ObjectMapper.Map<ShippingAddress, ShippingAddressDto>(order.ShippingAddress);
        return dto;
    }
}