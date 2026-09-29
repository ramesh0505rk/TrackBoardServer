using System;
using System.Collections.Generic;
using System.Text;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Infrastructure.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<bool> CheckOrgNameExists(string orgName, CancellationToken cancellationToken);
        Task<Guid> RegisterOrganization(OrganizationRegisterRequest request, CancellationToken cancellationToken);
    }
}
