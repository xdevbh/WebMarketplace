using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyCardDto : EntityDto<Guid>
{
    public string CompanyIdentificationNumber { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string? ShortDescription { get; set; }
    
    public byte[]? ImageContent { get; set; }
    
    public string ImageContentType { get; } = "application/octet-stream";
    
    public string City { get; set; }
    public string Country { get; set; }
}