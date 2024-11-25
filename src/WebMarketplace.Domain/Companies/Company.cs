using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace WebMarketplace.Companies;

public class Company : FullAuditedAggregateRoot<Guid>
{
    public virtual string CompanyIdentificationNumber { get; private set; }
    public virtual string Name { get; private set; }
    public virtual string DisplayName { get; private set; }
    public virtual Guid AddressId { get; set; }
    public virtual string? ShortDescription { get; set; }
    public virtual string? FullDescription { get; set; }
    public virtual string? Website { get; set; }

    public virtual List<CompanyImage> Images { get; set; }
    public virtual CompanyImage? DefaultImage => 
        Images?.Where(x => x.IsDefault).FirstOrDefault() ?? Images?.FirstOrDefault();

    public virtual List<CompanyBlogPost> BlogPosts { get; set; }              
    
    
    // todo: add contact information as list with ContactInformationType: mail, phone ... 

    protected Company() { }

    public Company(
        Guid id, 
        string companyIdentificationNumber,
        string name, 
        string displayName, 
        Guid addressId, 
        string? shortDescription, 
        string? fullDescription, 
        string? website)
    :base(id)
    {
        SetIdentificationNumber(companyIdentificationNumber);
        SetName(name);
        SetDisplayName(displayName);
        AddressId = addressId;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        Website = website;

        Images = new List<CompanyImage>();
        BlogPosts = new List<CompanyBlogPost>();
    }
    
    public Company SetIdentificationNumber(string companyIdentificationNumber)
    {
        CompanyIdentificationNumber  = Check.NotNullOrWhiteSpace(companyIdentificationNumber, nameof(companyIdentificationNumber));
        return this;
    }

    public Company SetName(string name)
    {
        Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        return this;
    }
    
    public Company SetDisplayName(string displayName)
    {
        DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName));
        return this;
    }


    #region Images

    public Company AddImage(
        string blobName,
        bool isDefault)
    {
        var defaultImage = Images.FirstOrDefault(x => x.IsDefault);
        if (defaultImage is not null && isDefault)
        {
            defaultImage.IsDefault = false;
        }

        var image = new CompanyImage(this.Id, blobName, isDefault);
        Images.Add(image);
        return this;
    }

    public Company SetDefaultImage(
        string blobName,
        bool isDefault)
    {
        var defaultImage = Images.FirstOrDefault(x => x.IsDefault);
        if (defaultImage is not null && isDefault)
        {
            defaultImage.IsDefault = false;
        }

        var image = Images.FirstOrDefault(x => x.BlobName == blobName);
        if (image is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyImageNotFound);
        }

        image.IsDefault = isDefault;

        return this;
    }

    public Company RemoveImage(string blobName)
    {
        var image = Images.FirstOrDefault(x => x.BlobName == blobName);
        if (image is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyImageNotFound);
        }
        else
        {
            if (image.IsDefault)
            {
                throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyImageDefaultRemoveNotAllowed);
            }
        }

        Images.Remove(image);
        return this;
    }

    #endregion
    
    #region BlogPosts
    
    public Company AddBlogPost(
        Guid blogPostId,
        string title,
        string content,
        bool isPublished)
    {
        var blogPost = new CompanyBlogPost(blogPostId, this.Id, title, content, isPublished);
        BlogPosts.Add(blogPost);
        return this;
    }
    
    public Company UpdateBlogPost(
        Guid blogPostId,
        string title,
        string content,
        bool isPublished)
    {
        var blogPost = BlogPosts.FirstOrDefault(x => x.Id == blogPostId);
        if (blogPost is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyBlogPostNotFound);
        }

        blogPost.SetTitle(title);
        blogPost.SetContent(content);
        blogPost.IsPublished = isPublished;
        return this;
    }
    
    public Company RemoveBlogPost(Guid blogPostId)
    {
        var blogPost = BlogPosts.FirstOrDefault(x => x.Id == blogPostId);
        if (blogPost is null)
        {
            throw new BusinessException(WebMarketplaceDomainErrorCodes.CompanyBlogPostNotFound);
        }

        BlogPosts.Remove(blogPost);
        return this;
    }
    
    #endregion

}