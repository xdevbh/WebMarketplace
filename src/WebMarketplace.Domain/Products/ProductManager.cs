using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using WebMarketplace.Companies;
using WebMarketplace.Currencies;

namespace WebMarketplace.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly ICurrencyRepository _currencyRepository;

        public ProductManager(
            IRepository<Product, Guid> productRepository, 
            ICurrencyRepository currencyRepository)
        {
            _productRepository = productRepository;
            _currencyRepository = currencyRepository;
        }

        #region Verify

        public async Task VerifyNameAsync(string name)
        {
        }

        public async Task VerifyCurrencyAsync(string currency)
        {
            Check.NotNullOrWhiteSpace(currency, nameof(currency));
            Check.Length(currency, nameof(currency), WebMarketplaceConsts.CurrencyCodeLength, WebMarketplaceConsts.CurrencyCodeLength);

            if (await _currencyRepository.Exists(currency))
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CurrencyNotFound).WithData("Code", currency);
            }
        }

        #endregion

        public async Task<Product> CreateAsync(
            Guid companyId,
            string name,
            ProductCategory productCategory,
            ProductType productType,
            string? shortDescription,
            string? fullDescription)
        {
            var product = new Product(
                GuidGenerator.Create(),
                companyId,
                name,
                productCategory,
                productType,
                shortDescription,
                fullDescription);
            return product;
        }

        public async Task<Product> EditAsync(
            Product product,
            string name,
            ProductCategory productCategory,
            ProductType productType,
            string? shortDescription,
            string? fullDescription)
        {
            product.SetName(name);
            product.ProductCategory = productCategory;
            product.ProductType = productType;
            product.ShortDescription = shortDescription;
            product.FullDescription = fullDescription;

            return product;
        }

        #region Reviews

        public async Task AddProductReviewAsync(
            Product product,
            Guid userId,
            int rating,
            string? comment)
        {
            product.AddReview(GuidGenerator.Create(), userId, rating, comment);
        }

        public async Task UpdateProductReviewAsync(
            Product product,
            Guid reviewId,
            int rating,
            string? comment)
        {
            product.UpdateReview(reviewId, rating, comment);
        }

        public async Task RemoveProductReviewAsync(
            Product product,
            Guid reviewId)
        {
            product.RemoveReview(reviewId);
        }

        #endregion

        #region Prices

        public async Task AddProductPriceAsync(
            Product product,
            DateTime date,
            decimal amount,
            string currency)
        {
            currency = currency.ToUpper();
            product.AddPrice(date, amount, currency);
        }

        #endregion
    }
}