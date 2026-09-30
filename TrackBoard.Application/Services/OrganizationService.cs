using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackBoard.Application.Interfaces;
using TrackBoard.Application.ResponseDTOs;
using TrackBoard.Domain.Common.ExceptionHandling;
using TrackBoard.Domain.Entities;
using TrackBoard.Infrastructure.Interfaces;

namespace TrackBoard.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly IOrganizationRepository _organizationRepository;
        public async Task<OrganizationRegisterDTO> Register(OrganizationRegisterRequest request, CancellationToken cancellationToken)
        {
            await CheckOrgNameExists(request.OrganizationName, cancellationToken);
            var orgId = RegisterOrganization(request, cancellationToken);
        }

        private async Task CheckOrgNameExists(string orgName, CancellationToken cancellationToken)
        {
            try
            {
                var orgNameExists = await _organizationRepository.CheckOrgNameExists(orgName, cancellationToken);
                if (orgNameExists)
                {
                    _logger.LogError("Organization with name {OrgName} already exists.", orgName);
                    throw new BadRequestCustomException(["Organization with this name already exists."]);
                }
            }
            catch (Exception ex)
            {
                var inputParams = new { orgName };
                _logger.LogError(ex, "Error thrown in OrganizationService.CheckOrgNameExists. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        private async Task<bool> CheckAlreadyAMember(string userId, CancellationToken cancellationToken)
        {
            try
            {
                var alreadyAMember = await _organizationRepository.
            }
            catch (Exception ex)
            {
                var inputParams = new { userId };
                _logger.LogError(ex, "Error thrown in OrganizationService.CheckAlreadyAMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        private async Task<Guid> RegisterOrganization(OrganizationRegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var orgId = await _organizationRepository.RegisterOrganization(request, cancellationToken);
                return orgId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in OrganizationService.RegisterOrganization. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
                throw;
            }
        }

        private async Task<bool> AddOrganizationMember(string orgId, string userId, string role)
        {
            try
            {
                var memberAdded = await _organizationRepository.AddOrganizationMember(orgId, userId, role);
                return memberAdded;
            }
            catch (Exception ex)
            {
                var inputParams = new { orgId, userId, role };
                _logger.LogError(ex, "Error thrown in OrganizationService.AddOrganizationMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }
    }
}
