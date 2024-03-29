﻿using System;
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
        private readonly UserVendorManager _userVendorManager;

        public ProductManager(
            IRepository<Product, Guid> productRepository,
            UserVendorManager userVendorManager)
        {
            _productRepository = productRepository;
            _userVendorManager = userVendorManager;
        }

        #region Rating

        public async Task UpdateRating(Guid productId, double rating)
        {
            var product = await _productRepository.GetAsync(productId);
            product.AverageRating = rating;
            await _productRepository.UpdateAsync(product);
        }
        
        public async Task UpdateRating(Product product, double rating)
        {
            product.AverageRating = rating;
            await _productRepository.UpdateAsync(product);
        }

        #endregion
        
        
        public async Task<bool> HasEditPermissionAsync(Product product, Guid? userId)
        {
            if (userId == null)
            {
                return false;
            }
            
            var vendor = await _userVendorManager.GetVendorByUserAsync(userId.Value);
            if (vendor == null)
            {
                return false;
            }

            if (product.VendorId == vendor.Id)
                return true;

            return false;
        }


        public async Task VerifyAsync(Product product)
        {

        }
    }
}
