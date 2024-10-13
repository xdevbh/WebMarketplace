using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Content;

namespace WebMarketplace.Companies;

public class CreateCompanyImageDto
{
    public string FileName { get; set; }

    public byte[] Content { get; set; }

    public bool IsDefault { get; set; } = false;
}