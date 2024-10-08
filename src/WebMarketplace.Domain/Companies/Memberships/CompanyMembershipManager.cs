using System;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Identity;

namespace WebMarketplace.Companies.Memberships;

public class CompanyMembershipManager : DomainService
{
    private readonly IRepository<CompanyMembership, Guid> _companyMembershipRepository;

    public CompanyMembershipManager(IRepository<CompanyMembership, Guid> companyMembershipRepository)
    {
        _companyMembershipRepository = companyMembershipRepository;
    }

    public async Task  JoinAsync(
        Guid companyId,
        Guid userId)
    {
        if (await IsJoinedAsync(companyId, userId))
        {
            return;
        }

        var membership = await _companyMembershipRepository.InsertAsync(
            new CompanyMembership(
                GuidGenerator.Create(),
                companyId,
                userId
            )
        );


    }

    public async Task<bool> IsJoinedAsync(
        Guid companyId,
        Guid userId)
    {
        return await _companyMembershipRepository
            .AnyAsync(x => x.CompanyId == companyId && x.UserId == userId);
    }
}