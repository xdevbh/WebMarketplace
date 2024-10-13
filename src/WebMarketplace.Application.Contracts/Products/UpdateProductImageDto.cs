using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMarketplace.Products
{
    public class UpdateProductImageDto
    {
        public Guid ProductId { get; set; }

        public string BlobName { get; set; }

        public bool IsDefault { get; set; }
    }
}
