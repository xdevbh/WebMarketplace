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
        private readonly IRepository<UserVendor, Guid> _userVendorRepository;

        public ProductManager(
            IRepository<Product, Guid> productRepository,
            IRepository<UserVendor, Guid> userVendorRepository)
        {
            _productRepository = productRepository;
            _userVendorRepository = userVendorRepository;
        }

        public async Task AssignAsync(Product product, Guid? userId)
        {
            if (userId == null)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductAssignmentException);
            }

            var userVendor = await _userVendorRepository.FirstOrDefaultAsync(x => x.UserId == userId.Value);
            if (userVendor == null)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductAssignmentException);
            }

            product.VendorId = userVendor.VendorId;
        }

        public async Task<bool> HasEditPermissionAsync(Product product, Guid? userId)
        {
            var userVendor = await _userVendorRepository.FirstOrDefaultAsync(x => x.UserId == userId.Value);
            if (userVendor == null)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductAssignmentException);
            }

            if (product.VendorId != userVendor.VendorId)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductAssignmentException);
            }

            return true;
        }


        public async Task VerifyAsync(Product product)
        {

        }
    }
}
