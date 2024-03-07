using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Volo.Abp;
using WebMarketplace.Common;
using WebMarketplace.Orders;
using WebMarketplace.ProductCategories;
using WebMarketplace.Reviews;
using WebMarketplace.Vendors;

namespace WebMarketplace.Products;

public class ProductAppService : WebMarketplaceAppService, IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<UserVendor, Guid> _userVendorRepository;
    private readonly IRepository<Review, Guid> _reviewRepository;
    private readonly IRepository<OrderItem, Guid> _orderItemRepository;
    private readonly IRepository<Vendor, Guid> _vendorRepository;
    private readonly IRepository<ProductCategory, Guid> _categoryRepository;


    private readonly ProductManager _productManager;


    public ProductAppService(
        ProductManager productManager,
        IRepository<Product, Guid> productRepository,
        IRepository<UserVendor, Guid> userVendorRepository,
        IRepository<Review, Guid> reviewRepository,
        IRepository<OrderItem, Guid> orderItemRepository,
        IRepository<Vendor, Guid> vendorRepository)
    {
        _productManager = productManager;
        _productRepository = productRepository;
        _userVendorRepository = userVendorRepository;
        _reviewRepository = reviewRepository;
        _orderItemRepository = orderItemRepository;
        _vendorRepository = vendorRepository;
    }

    #region CRUD_Operations

    [Authorize(WebMarketplacePermissions.Products.Default)]
    public async Task<PagedResultDto<ProductDto>> GetFilteredListAsync(ProductRequestDto input)
    {
        var query = await _productRepository.GetQueryableAsync();

        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);
        query = ApplyFilter(query, input);

        var queryResult = await AsyncExecuter.ToListAsync(query);
        var productsDtos = queryResult.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();
        var totalCount = queryResult.Count;

        return new PagedResultDto<ProductDto>(
            totalCount,
            productsDtos
        );
    }

    [Authorize(WebMarketplacePermissions.Products.Create)]
    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var user = CurrentUser;
        var product = ObjectMapper.Map<CreateUpdateProductDto, Product>(input);
        await _productRepository.InsertAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(WebMarketplacePermissions.Products.Edit)]
    public async Task UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);
        var user = CurrentUser;
        var canUpdate = await _productManager.HasEditPermissionAsync(product, user.Id);
        if (!canUpdate)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductUpdateException);
        }

        var newProduct = ObjectMapper.Map(input, product);
        await _productRepository.UpdateAsync(newProduct);
    }

    [Authorize(WebMarketplacePermissions.Products.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        var user = CurrentUser;
        var canDelete = await _productManager.HasEditPermissionAsync(product, user.Id);
        if (canDelete)
        {
            await _productRepository.DeleteAsync(id);
        }
        else
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductDeleteException);
        }
    }
    

    [AllowAnonymous]
    public async Task<PagedResultDto<ProductViewDto>> GetFilteredViewListAsync(ProductRequestDto input)
    {
        var query = await _productRepository.GetQueryableAsync();
        query = ApplyFilter(query, input);
        query = ApplySorting(query, input);
        var totalCount = await AsyncExecuter.CountAsync(query);
        query = ApplyPaging(query, input);
        var products = await AsyncExecuter.ToListAsync(query);
        var views = ObjectMapper.Map<List<Product>, List<ProductViewDto>>(products);

        var categoryIds = products
            .Where(x => x.ProductCategoryId.HasValue)
            .Select(x => x.ProductCategoryId.Value)
            .Distinct().ToList();
        var categories = await _categoryRepository.GetListAsync(x=> categoryIds.Contains(x.Id));
        var categoryDtos = ObjectMapper.Map<List<ProductCategory>, List<ProductCategoryDto>>(categories);
        
        var vendorIds = products
            .Select(x => x.VendorId)
            .Distinct().ToList();
        var vendors = await _vendorRepository.GetListAsync(x=> vendorIds.Contains(x.Id));
        var vendorDtos = ObjectMapper.Map<List<Vendor>, List<VendorDto>>(vendors);
        
        foreach (var product in views)
        {
            var category = categoryDtos.FirstOrDefault(x=>x.Id == product.ProductCategoryId);

            var reviews = await _reviewRepository.GetListAsync(x => x.ProductId == product.Id);
            product.Reviews = ObjectMapper.Map<List<Review>, List<ReviewDto>>(reviews);
            product.AverageRating = reviews.Average(x => x.Rating);

            var soldCount = await _orderItemRepository.CountAsync(x => x.ProductId == product.Id);
            product.SoldCount = soldCount;

            var vendor = vendorDtos.FirstOrDefault(x=>x.Id == product.VendorId);
        }
        
        return new PagedResultDto<ProductViewDto>(
            totalCount,
            views
        );
    }

    public Task<PagedResultDto<ProductCardDto>> GetVendorProductCardListAsync(PagedAndSortedResultRequestDto input)
    {
        throw new NotImplementedException();
    }

    [AllowAnonymous]
    public async Task<ProductDto> GetAsync(Guid id)
    {
        var author = await _productRepository.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(author);
    }

    #endregion

    [AllowAnonymous]
    public async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        if (input.Sorting.IsNullOrWhiteSpace())
        {
            input.Sorting = nameof(Product.Name);
        }

        var query = await _productRepository.GetQueryableAsync();
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);
        var queryResult = await AsyncExecuter.ToListAsync(query);
        var productsDtos = queryResult.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();
        var totalCount = productsDtos.Count();

        return new PagedResultDto<ProductDto>(
            totalCount,
            productsDtos
        );
    }

    [AllowAnonymous]
    public async Task<PagedResultDto<ProductDto>> GetListByVendorAsync(PagedAndSortedResultRequestDto input, Guid vendorId)
    {
        var queryable = await _productRepository.GetQueryableAsync();

        var query = queryable.Where(x => x.VendorId == vendorId);
        var totalCount = await AsyncExecuter.CountAsync(query);
        
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var queryResult = await AsyncExecuter.ToListAsync(query);
        var productsDtos = queryResult.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();
        

        return new PagedResultDto<ProductDto>(
            totalCount,
            productsDtos
        );
    }

    private IQueryable<Product> ApplyPaging(IQueryable<Product> query, PagedAndSortedResultRequestDto input)
    {
        return query
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);
    }

    private IQueryable<Product> ApplySorting(IQueryable<Product> query, PagedAndSortedResultRequestDto input)
    {
        if (!string.IsNullOrEmpty(input.Sorting))
        {
            query = query.OrderBy(input.Sorting);
        }

        return query;
    }

    private IQueryable<Product> ApplyFilter(IQueryable<Product> query, ProductRequestDto input)
    {
        if (input.VendorId != null)
        {
            query = query.Where(x => x.VendorId == input.VendorId);
        }

        if (input.Name != null)
        {
            query = query.Where(x => x.Name.Contains(input.Name));
        }

        if (input.Price != null)
        {
            query = query.Where(x => x.Price == input.Price);
        }

        if (input.Currency != null)
        {
            query = query.Where(x => x.Currency == input.Currency);
        }

        if (input.ProductCategoryId != null)
        {
            query = query.Where(x => x.ProductCategoryId == input.ProductCategoryId);
        }

        if (input.InStock != null)
        {
            query = query.Where(x => x.InStock == input.InStock);
        }

        if (input.IsPublished != null)
        {
            query = query.Where(x => x.IsPublished == input.IsPublished);
        }

        return query;
    }

    #region Common

    

    #endregion

    #region ForVendors

    

    #endregion

    #region ForCustomers
    // always is published = true
    [AllowAnonymous]
    public Task<PagedResultDto<ProductCardDto>> GetFilteredProductCardListAsync(ProductRequestDto input)
    {
        throw new NotImplementedException();
    }
    
    [AllowAnonymous]
    public Task<ProductViewDto> GetProductViewAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    #endregion
}