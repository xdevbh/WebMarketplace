using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Content;

namespace WebMarketplace.Companies;

public class CompanyImageDto : EntityDto
{
    public Guid CompanyId { get; set; }

    public string BlobName { get; set; }

    public bool IsDefault { get; set; }

    public byte[] Content { get; set; }

    public string ContentType { get; } = "application/octet-stream";
}