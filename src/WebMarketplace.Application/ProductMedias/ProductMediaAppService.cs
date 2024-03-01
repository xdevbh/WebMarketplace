using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Domain.Repositories;
using WebMarketplace.BlobContainers;
using WebMarketplace.ProductMedias;
using System.Linq;
using Volo.Abp.Content;
using WebMarketplace.Products;

namespace WebMarketplace.ProductCategories;

public class ProductMediaAppService : WebMarketplaceAppService, IProductMediaAppService
{
    private readonly IBlobContainer<ProductImageContainer> _blobContainer;
    private readonly IRepository<ProductMedia, Guid> _productMediaRepository;
    private readonly IRepository<DatabaseBlob, Guid> _databaseBlobRepository;

    public ProductMediaAppService(
        IRepository<ProductMedia, Guid> productMediaRepository,
        IRepository<DatabaseBlob, Guid> databaseBlobRepository,
        IBlobContainer<ProductImageContainer> blobContainer)
    {
        _productMediaRepository = productMediaRepository;
        _blobContainer = blobContainer;
        _databaseBlobRepository = databaseBlobRepository;
    }

    public async Task<List<ProductMediaDto>> GetListByProductIdAsync(Guid productId)
    {
        var result = new List<ProductMediaDto>();

        var medias = await _productMediaRepository.GetListAsync(x=>x.ProductId == productId);
        foreach (var media in medias)
        {
                var dto = ObjectMapper.Map<ProductMedia, ProductMediaDto>(media);
                dto.Content = await _blobContainer.GetAllBytesAsync(media.Id.ToString());
                result.Add(dto);
        }

        return result;
    }
    
    public async Task<ListResultDto<ProductMediaDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var result = new List<ProductMediaDto>();

        var medias = await _productMediaRepository.GetListAsync();
        foreach (var media in medias)    
        {
            var dto = ObjectMapper.Map<ProductMedia, ProductMediaDto>(media);
            dto.Content = await _blobContainer.GetAllBytesAsync(media.Id.ToString());
            result.Add(dto);
        }

        return new ListResultDto<ProductMediaDto>(result);
    }

    public async Task<ProductMediaDto> GetAsync(Guid id)
    {
        var media = await _productMediaRepository.GetAsync(id);
        var dto = ObjectMapper.Map<ProductMedia, ProductMediaDto>(media);
        dto.Content = await _blobContainer.GetAllBytesAsync(media.Id.ToString());
        return dto;
    }

    public async Task<ProductMediaDto> CreateAsync(CreateProductMediaDto input)
    {
        var productMedia = ObjectMapper.Map<CreateProductMediaDto, ProductMedia>(input);
        await _productMediaRepository.InsertAsync(productMedia);
        await _blobContainer.SaveAsync(productMedia.Id.ToString(), input.Content);
        return ObjectMapper.Map<ProductMedia, ProductMediaDto>(productMedia);
    }

    public async Task UpdateAsync(Guid id, UpdateProductMediaDto input)
    {
        var media = await _productMediaRepository.GetAsync(id);
        var newMedia = ObjectMapper.Map(input, media);
        await _productMediaRepository.UpdateAsync(newMedia);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _blobContainer.DeleteAsync(id.ToString());
        await _productMediaRepository.DeleteAsync(id);
    }
}