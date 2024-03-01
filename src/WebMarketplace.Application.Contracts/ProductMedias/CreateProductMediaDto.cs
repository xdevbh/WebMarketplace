using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace WebMarketplace.ProductCategories;

public class CreateProductMediaDto
{
    // [Required] public CreateBlobDto Blob { get; set; }

    [Required] public Guid ProductId { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
    public byte[] Content { get; set; }
}