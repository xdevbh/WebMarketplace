using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Companies;

public class CreateUpdateCompanyBlogPostSellerDto 
{ 
    public string Title { get; set; }
    
    public string Content { get; set; }
}