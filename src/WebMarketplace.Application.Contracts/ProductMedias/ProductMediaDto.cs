using System;
using System.IO;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.ProductCategories;

public class ProductMediaDto : AuditedEntityDto<Guid>
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
    public byte[] Content { get; set; }
}