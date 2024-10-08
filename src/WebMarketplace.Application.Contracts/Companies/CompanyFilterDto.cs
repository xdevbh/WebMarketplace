using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyFilterDto: PagedAndSortedResultRequestDto
{
    public Guid? Id { get; set; }
    public string? CompanyIdentificationNumber { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
}