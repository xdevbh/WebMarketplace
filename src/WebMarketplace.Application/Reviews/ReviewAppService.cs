using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace WebMarketplace.Reviews;

public class ReviewAppService : CrudAppService
    <Review, ReviewDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateReviewDto>, IReviewAppService
{
    public ReviewAppService(IRepository<Review, Guid> repository)
        : base(repository)
    {
    }
}