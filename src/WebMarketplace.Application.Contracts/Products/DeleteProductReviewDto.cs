using System;

namespace WebMarketplace.Products;

public class DeleteProductReviewDto
{
    public Guid ProductId { get; set; }
    public Guid ReviewId { get; set; }
}