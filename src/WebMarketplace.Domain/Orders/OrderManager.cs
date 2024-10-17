using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;
using WebMarketplace.Currencies;

namespace WebMarketplace.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public OrderManager(
            IOrderRepository orderRepository, 
            ICurrencyRepository currencyRepository)
        {
            _orderRepository = orderRepository;
            _currencyRepository = currencyRepository;
        }

        #region Verify

        public async Task VerifyCurrencyAsync(string currency)
        {
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength, WebMarketplaceConsts.CurrencyCodeLength);

            if (await _currencyRepository.Exists(currency))
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CurrencyNotFound).WithData("Code", currency);
            }
        }

        public async Task VerifyUnitPriceAsync(decimal price)
        {
            if (price < 0)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.PriceNotNegative);
            }
        }

        public async Task VerifyQuantityAsync(int quantity)
        {
            Check.Positive(quantity, nameof(quantity));
        }
        
        public async Task<bool> CanCancelAsync(Order order)
        {
            return order.Status.IsIn(OrderStatus.New, OrderStatus.PendingPayment, OrderStatus.Paid, OrderStatus.Processing);
        } 

        #endregion

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
                var currency = orderItem.currency.ToUpper();
                await VerifyCurrencyAsync(currency);

                var unitPrice = orderItem.unitPrice;
                await VerifyUnitPriceAsync(unitPrice);

                var quantity = orderItem.quantity;
                await VerifyQuantityAsync(quantity);

                order.AddItem(
                    GuidGenerator.Create(), 
                    orderItem.productId,
                    orderItem.productName,
                    quantity,
                    unitPrice,
                    currency
                );
            }

            return await _orderRepository.InsertAsync(order);
        }

        public async Task<Order> ChangeStatusAsync(Guid orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if(order == null)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderNotFound).WithData("Id", orderId);
            }
            
            if(await CanCancelAsync(order))
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.OrderCannotBeCancelled).WithData("Id", orderId);
            }
            
            order.ChangeStatus(status);
            return await _orderRepository.UpdateAsync(order);
        }
    }
}