using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyBlogPostFilterDto: PagedAndSortedResultRequestDto
{
    public Guid CompanyId { get; set; }
}