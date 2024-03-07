using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using WebMarketplace.Products;

namespace WebMarketplace.Reviews;

public class ReviewManager : DomainService
{
    private readonly IRepository<Review, Guid> _reviewRepository;
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly ProductManager _productManager;
    public ReviewManager(
        IRepository<Review, Guid> reviewRepository,
        IRepository<Product, Guid> productRepository,
        ProductManager productManager)
    {
        _reviewRepository = reviewRepository;
        _productRepository = productRepository;
        _productManager = productManager;
    }

    public async Task<Review> CreateAsync(Review review)
    {
        var product = await _productRepository.GetAsync(review.ProductId);
        if (product == null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.ProductNotFoundException);
        }

        await _reviewRepository.InsertAsync(review);

        // var reviewQuery = await _reviewRepository.GetQueryableAsync();
        // var query = reviewQuery.Where(r => r.ProductId == review.ProductId).Average(r => r.Rating);
        // var averageRating = await AsyncExecuter.FirstOrDefaultAsync<double>(query);

        var reviews = await _reviewRepository.GetListAsync(r => r.ProductId == review.ProductId);
        var averageRating = reviews.Average(r => r.Rating);
        
        await _productManager.UpdateRating(review.ProductId, averageRating);
        return review;
    }
}