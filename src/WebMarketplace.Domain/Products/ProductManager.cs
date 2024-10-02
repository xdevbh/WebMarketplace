using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using WebMarketplace.Vendors;

namespace WebMarketplace.Products
{
    public class ProductManager : DomainService
    {
        private readonly IRepository<Product, Guid> _productRepository;

        public ProductManager(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        #region Verify

        public async Task VerifyNameAsync(string name)
        {
        }

        #endregion

        public async Task<Product> CreateAsync(
            Guid vendorId,
            string name,
            ProductCategory productCategory,
            ProductType productType,
            string? shortDescription,
            string? fullDescription)
        {
            var product = new Product(
                GuidGenerator.Create(),
                vendorId,
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
            double rating,
            string? comment)
        {
            product.AddReview(GuidGenerator.Create(), userId, rating, comment);
        }

        public async Task UpdateProductReviewAsync(
            Product product,
            Guid reviewId,
            double rating,
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
            product.AddPrice(date, amount, currency);
        }

        #endregion
    }
}