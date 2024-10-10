using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Products
{
    public class ProductPriceListFilterDto : PagedAndSortedResultRequestDto
    {
        public Guid ProductId { get; set; }
    }
}
