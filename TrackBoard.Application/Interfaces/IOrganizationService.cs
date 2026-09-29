using System;
using System.Collections.Generic;
using System.Text;
using TrackBoard.Application.ResponseDTOs;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Application.Interfaces
{
    public interface IOrganizationService
    {
        Task<OrganizationRegisterDTO> Register(OrganizationRegisterRequest request, CancellationToken cancellationToken);
    }
}
