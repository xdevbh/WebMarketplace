using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Companies;
using WebMarketplace.Companies.Memberships;

namespace WebMarketplace.Products;

[Authorize("AdminOnly")]
public class ProductAdminAppService : WebMarketplaceAppService, IProductAdminAppService
{
    private readonly IProductRepository _productRepository;
    private readonly ProductManager _productManager;
    private readonly IRepository<Company, Guid> _companyRepository;
    private readonly IRepository<CompanyMembership, Guid> _vendorUserRepository;
    private readonly IBlobContainer _productBlobContainer;

    public ProductAdminAppService(IProductRepository productRepository, ProductManager productManager, IRepository<Company, Guid> companyRepository, IRepository<CompanyMembership, Guid> vendorUserRepository, IBlobContainer productBlobContainer)
    {
        _productRepository = productRepository;
        _productManager = productManager;
        _companyRepository = companyRepository;
        _vendorUserRepository = vendorUserRepository;
        _productBlobContainer = productBlobContainer;
    }
    public Task DeleteReview(DeleteProductReviewDto input)
    {
        throw new NotImplementedException();
    }

    public Task DeleteProduct(Guid id)
    {
        throw new NotImplementedException();
    }

    #region Images
    
    public async Task<ProductImageDto> GetDefaultImageAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);
    
        if (product != null || product.DefaultImage == null)
        {
            return null; 
        }

        var blob = await _productBlobContainer.GetAllBytesOrNullAsync(product.DefaultImage.BlobName);
        var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(product.DefaultImage);
        dto.Content = blob;
        return dto;
    }

    public async Task<ListResultDto<ProductImageDto>> GetAllImagesAsync(Guid productId)
    {
        var product = await _productRepository.GetAsync(productId);

        if (product == null || product.Images == null || !product.Images.Any())
        {
            return new ListResultDto<ProductImageDto>();
        }

        var imageDtos = new List<ProductImageDto>();

        foreach (var image in product.Images)
        {
            var blob = await _productBlobContainer.GetAllBytesOrNullAsync(image.BlobName);
            var dto = ObjectMapper.Map<ProductImage, ProductImageDto>(image);
            dto.Content = blob;
            imageDtos.Add(dto);
        }

        return new ListResultDto<ProductImageDto>(imageDtos);
    }

    public async Task CreateProductImageAsync(CreateProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);
        
        var blobName = GuidGenerator.Create().ToString();
        product.AddImage(blobName, input.IsDefault);
        
        await _productBlobContainer.SaveAsync(blobName, input.Content);
    }

    public async Task DeleteProductImageAsync(DeleteProductImageDto input)
    {
        var product = await _productRepository.GetAsync(input.ProductId);
        
        product.RemoveImage(input.BlobName);
        await _productBlobContainer.DeleteAsync(input.BlobName);
    }

    #endregion

    // can edit product with Company ID
}