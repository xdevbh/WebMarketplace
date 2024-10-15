using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMarketplace.Products
{
    public static class ProductConsts
    { 
        public const int ProductImageMaxFileSize = 1 * 1024 * 1024;
        public static string[] ProductImageAllowedExtensions = { ".jpg", ".png" };
        
        public const int RatingMinValue = 0;
        public const int RatingMaxValue = 5;
    }
}
