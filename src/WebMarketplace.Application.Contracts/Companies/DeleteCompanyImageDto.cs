using System;

namespace WebMarketplace.Companies;

public class DeleteCompanyImageDto
{
    public Guid CompanyId { get; set; }

    public string BlobName { get; set; }
}