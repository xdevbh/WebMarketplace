using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace WebMarketplace.Blazor.Client.Components;

public abstract class WmDataGridPageBase : WmPageBase
{
    protected bool CanView { get; set; } = true;
    protected bool CanCreate { get; set; } = false;
    protected bool CanEdit { get; set; } = false;
    protected bool CanDelete { get; set; } = false;

    protected int PageSize { get; set; } = LimitedResultRequestDto.DefaultMaxResultCount;
    protected int CurrentPage { get; set; }
    protected string CurrentSorting { get; set; }
    protected int TotalCount { get; set; }
    
    protected abstract Task GetDataAsync();

    protected virtual Task ViewAsync(Guid id)
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task CreateAsync()
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task EditAsync(Guid id)
    {
        return Task.CompletedTask;
    }
    
    protected virtual Task DeleteAsync(Guid id)
    {
        return Task.CompletedTask;
    }
}