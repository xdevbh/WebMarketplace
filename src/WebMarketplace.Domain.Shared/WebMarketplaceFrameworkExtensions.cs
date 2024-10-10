using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMarketplace
{
    public static class WebMarketplaceFrameworkExtensions
    {
        public static string ToText<TEnum>(this TEnum value) where TEnum : Enum
        {
            return $"Enum:{Enum.GetName(typeof(TEnum), value)}:{value.ToString()}";
        }
    }
}
