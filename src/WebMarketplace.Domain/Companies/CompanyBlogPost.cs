using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Companies;

public class CompanyBlogPost : AuditedEntity<Guid>
{
    public Guid CompanyId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public bool IsPublished { get; set; }

    protected CompanyBlogPost()
    {
    }

    public CompanyBlogPost(
        Guid id,
        Guid companyId,
        string title,
        string content,
        bool isPublished)
        : base(id)
    {
        CompanyId = companyId;
        SetTitle(title);
        SetContent(content);
        IsPublished = isPublished;
    }
    
    public CompanyBlogPost SetTitle(string title)
    {
        Title = title;
        return this;
    }
    
    public CompanyBlogPost SetContent(string content)
    {
        Content = content;
        return this;
    }
    
    public CompanyBlogPost Publish(bool isPublished)
    {
        IsPublished = isPublished;
        return this;
    }
}