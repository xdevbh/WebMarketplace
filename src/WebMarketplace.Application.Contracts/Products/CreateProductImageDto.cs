using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace WebMarketplace.Products;

public class CreateProductImageDto
{
    [Required]
    public Guid ProductId { get; set; }

    public string FileName { get; set; }

    public byte[] Content { get; set; }

    public bool IsDefault { get; set; } = false;

    //public IRemoteStreamContent ImageStreamContent { get; set; }
}