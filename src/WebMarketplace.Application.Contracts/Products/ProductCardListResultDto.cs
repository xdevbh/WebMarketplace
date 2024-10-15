using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products;

public class ProductCardListResultDto : PagedResultDto<ProductCardDto>
{
    public decimal MinPriceAmount { get; set; }
    public decimal MaxPriceAmount { get; set; }
    public double MinRating { get; set; }
    public double MaxRating { get; set; }
    public string PriceCurrency { get; set; }
}