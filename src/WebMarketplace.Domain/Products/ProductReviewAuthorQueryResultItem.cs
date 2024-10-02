using Volo.Abp.Identity;

namespace WebMarketplace.Products;

public class ProductReviewAuthorQueryResultItem
{
    public ProductReview Review { get; set; }
    public IdentityUser User { get; set; }

    public ProductReviewAuthorQueryResultItem(ProductReview review, IdentityUser user)
    {
        Review = review;
        User = user;
    }
}