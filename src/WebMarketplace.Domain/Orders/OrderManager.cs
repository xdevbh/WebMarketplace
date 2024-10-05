using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace WebMarketplace.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Order> CreateAsync(
            Guid buyerId,
            Guid addressId,
            Guid companyId,
            string companyName,
            List<(Guid productId, string productName, int quantity, decimal unitPrice, string currency)> orderItems)
        {
            var order = new Order(
                GuidGenerator.Create(),
                buyerId,
                addressId,
                companyId,
                companyName
            );

            foreach (var orderItem in orderItems)
            {
                order.AddItem(
                    GuidGenerator.Create(),
                    orderItem.productId,
                    orderItem.productName,
                    orderItem.quantity,
                    orderItem.unitPrice,
                    orderItem.currency
                );
            }

            return await _orderRepository.InsertAsync(order);
        }

        public async Task<Order> ChangeStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetAsync(orderId);
            order.ChangeStatus(status);
            return await _orderRepository.UpdateAsync(order);
        }
    }
}