using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using WebMarketplace.Addresses;
using WebMarketplace.Companies;
using WebMarketplace.Currencies;
using WebMarketplace.Products;

namespace WebMarketplace.Orders
{
    public class OrderManager : DomainService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICurrencyRepository _currencyRepository;
        private readonly IRepository<Address, Guid> _addressRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICompanyRepository _companyRepository;

        public OrderManager(IOrderRepository orderRepository, ICurrencyRepository currencyRepository, IRepository<Address, Guid> addressRepository, IProductRepository productRepository, ICompanyRepository companyRepository)
        {
            _orderRepository = orderRepository;
            _currencyRepository = currencyRepository;
            _addressRepository = addressRepository;
            _productRepository = productRepository;
            _companyRepository = companyRepository;
        }

        #region Verify

        public async Task VerifyCurrencyAsync(string currency)
        {
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength, WebMarketplaceConsts.CurrencyCodeLength);

            var exist = await _currencyRepository.Exists(currency);
            if (!exist)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CurrencyNotFound).WithData("Code", currency);
            }
        }

        public async Task VerifyPriceAsync(decimal price)
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
        
        public async Task VerifyProductAsync(Guid productId)
        {
            var exist = await _productRepository.GetAsync(productId) != null;
            if (!exist)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductNotFound);
            }
        }
        
        public async Task VerifyAddressAsync(Guid productId)
        {
            var exist = await _addressRepository.GetAsync(productId) != null;
            if (!exist)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.AddressNotFound);
            }
        }
        
        public async Task VerifyCompanyAsync(Guid productId)
        {
            var exist = await _companyRepository.GetAsync(productId) != null;
            if (!exist)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyNotFound);
            }
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
            var OrderTotalPrice = orderItems.Sum(x => x.quantity * x.unitPrice);
            await VerifyPriceAsync(OrderTotalPrice);
            
            var OrderCurrency = orderItems.First().currency.ToUpper();
            await VerifyCurrencyAsync(OrderCurrency);

            await VerifyAddressAsync(addressId);

            await VerifyCompanyAsync(companyId);
            
            var order = new Order(
                GuidGenerator.Create(),
                buyerId,
                addressId,
                companyId,
                companyName,
                OrderTotalPrice,
                OrderCurrency
            );

            foreach (var orderItem in orderItems)
            {
                var currency = orderItem.currency.ToUpper();
                await VerifyCurrencyAsync(currency);
            
                var unitPrice = orderItem.unitPrice;
                await VerifyPriceAsync(unitPrice);
            
                var quantity = orderItem.quantity;
                await VerifyQuantityAsync(quantity);
                
                await VerifyProductAsync(orderItem.productId);
            
                order.AddItem(
                    GuidGenerator.Create(), 
                    orderItem.productId,
                    orderItem.productName,
                    quantity,
                    unitPrice,
                    currency
                );
            }

            return await _orderRepository.InsertAsync(order, true);
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
            return order;
        }
    }
}