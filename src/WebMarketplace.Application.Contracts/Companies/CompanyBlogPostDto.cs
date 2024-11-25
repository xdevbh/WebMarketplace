using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyBlogPostDto : EntityDto<Guid>
{
    public Guid CompanyId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPublished { get; set; }
}