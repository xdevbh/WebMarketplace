using System;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CompanyLookupDto : EntityDto<Guid>
{
    public string Name { get; set; }
}