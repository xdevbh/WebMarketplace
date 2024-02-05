using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace WebMarketplace.Reviews;

public interface IReviewAppService : ICrudAppService
    <ReviewDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateReviewDto>
{
}