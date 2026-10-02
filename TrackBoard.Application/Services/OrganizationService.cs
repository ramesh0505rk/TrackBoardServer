using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TrackBoard.Application.Interfaces;
using TrackBoard.Application.ResponseDTOs;
using TrackBoard.Domain.Common.ExceptionHandling;
using TrackBoard.Domain.Entities;
using TrackBoard.Infrastructure.Interfaces;
using TrackBoard.Infrastructure.Repositories;

namespace TrackBoard.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly ILogger<OrganizationService> _logger;
        private readonly ITokenService _tokenService;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public OrganizationService(ITokenService tokenService, IOrganizationRepository organizationRepository, IUserRepository userRepository, ILogger<OrganizationService> logger)
        {
            _logger = logger;
            _tokenService = tokenService;
            _organizationRepository = organizationRepository;
            _userRepository = userRepository;
        }

        public async Task<OrganizationRegisterDTO> Register(OrganizationRegisterRequest request, CancellationToken cancellationToken)
        {
            await CheckOrgNameExists(request.OrganizationName, cancellationToken);
            await CheckAlreadyAMember(request.UserId, cancellationToken);
            var orgId = await RegisterOrganization(request, cancellationToken);
            var created = await AddOrganizationMember(orgId, request.UserId, "Admin", cancellationToken);
            var user = await GetUserDetailsByUserId(request.UserId, cancellationToken);
            var accessToken = _tokenService.GenerateToken(user);
            return CreateRegisterSuccessResponse(created, accessToken);
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

        private async Task CheckAlreadyAMember(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var alreadyAMember = await _organizationRepository.CheckAlreadyAMember(userId, cancellationToken);
                if (alreadyAMember)
                {
                    _logger.LogError("This user is already a member of another organization. UserId: {UserId}", userId);
                    throw new BadRequestCustomException(["This user is already a member of another organization."]);
                }
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

        private async Task<bool> AddOrganizationMember(Guid orgId, Guid userId, string role, CancellationToken cancellationToken)
        {
            try
            {
                var memberAdded = await _organizationRepository.AddOrganizationMember(orgId, userId, role, cancellationToken);
                return memberAdded;
            }
            catch (Exception ex)
            {
                var inputParams = new { orgId, userId, role };
                _logger.LogError(ex, "Error thrown in OrganizationService.AddOrganizationMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        private async Task<User> GetUserDetailsByUserId(Guid? userId, CancellationToken cancellationToken)
        {
            try
            {
                var userDetails = await _userRepository.GetUserDetailsByUserId(userId, cancellationToken);
                return userDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in UserService.GetUserDetailsByUserId. Input parameters: {InputParams}", userId);
                throw;
            }
        }

        private OrganizationRegisterDTO CreateRegisterSuccessResponse(bool isOrgCreated, string accessToken)
        {
            return new OrganizationRegisterDTO
            {
                Created = isOrgCreated,
                AccessToken = accessToken,
                RequestId = Guid.NewGuid().ToString(),
                ResponseMessage = "Organization registered successfully"
            };
        }
    }
}
