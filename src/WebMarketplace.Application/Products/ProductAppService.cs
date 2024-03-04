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
using WebMarketplace.Vendors;

namespace WebMarketplace.Products;

public class ProductAppService : WebMarketplaceAppService, IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<UserVendor, Guid> _userVendorRepository;
    private readonly ProductManager _productManager;


    public ProductAppService(
        ProductManager productManager,
        IRepository<Product, Guid> productRepository,
        IRepository<UserVendor, Guid> userVendorRepository)
    {
        _productManager = productManager;
        _productRepository = productRepository;
        _userVendorRepository = userVendorRepository;
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
        await _productManager.AssignAsync(product, user.Id);
        await _productRepository.InsertAsync(product);

        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    [Authorize(WebMarketplacePermissions.Products.Edit)]
    public async Task UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);
        var newProduct = ObjectMapper.Map(input, product);
        await _productRepository.UpdateAsync(newProduct);
    }

    [Authorize(WebMarketplacePermissions.Products.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var user = CurrentUser;
        var product = await _productRepository.GetAsync(id);
        //await _productManager.HasEditPermissionAsync(product, user.Id);
        await _productRepository.DeleteAsync(id);
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
        var totalCount = queryResult.Count();

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
        query = ApplySorting(query, input);
        query = ApplyPaging(query, input);

        var queryResult = await AsyncExecuter.ToListAsync(query);
        var productsDtos = queryResult.Select(x => ObjectMapper.Map<Product, ProductDto>(x)).ToList();
        var totalCount = queryResult.Count();

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
}
