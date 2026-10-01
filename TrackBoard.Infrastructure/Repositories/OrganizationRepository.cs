using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using TrackBoard.Domain.Entities;
using TrackBoard.Infrastructure.Interfaces;
using TrackBoard.Infrastructure.Presistence;

namespace TrackBoard.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly ILogger<OrganizationRepository> _logger;

        public OrganizationRepository(IDbConnectionFactory dbConnectionFactory, ILogger<OrganizationRepository> logger)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _logger = logger;
        }

        public async Task<bool> CheckOrgNameExists(string orgName, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in OrganizationRepository.CheckUserExists. Input parameters: {orgName}", orgName);

                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var query = "SELECT COUNT(1) FROM Organizations WHERE [Name] = @OrgName";

                var parameters = new DynamicParameters();
                parameters.Add("@OrgName", orgName);

                var result = await connection.ExecuteScalarAsync<int>(query, parameters, commandType: CommandType.Text);

                return result > 0;
            }
            catch (Exception ex)
            {
                var inputParams = new { orgName };
                _logger.LogError(ex, "Error thrown in OrganizationRepository.CheckOrgNameExists. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        public async Task<bool> CheckAlreadyAMember(Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in OrganizationRepository.CheckAlreadyAMember. Input parameters: {userId}", userId);

                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var query = "SELECT COUNT(1) FROM OrganizationMembers WHERE UserId = @UserId";

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                var result = await connection.ExecuteScalarAsync<int>(query, parameters, commandType: CommandType.Text);

                return result > 0;
            }
            catch (Exception ex)
            {
                var inputParams = new { userId };
                _logger.LogError(ex, "Error thrown in OrganizationRepository.CheckAlreadyAMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }

        public async Task<Guid> RegisterOrganization(OrganizationRegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Information in OrganizationRepository.RegisterOrganization. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));

                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var query = @"INSERT INTO Organizations(Id, [Name]) 
                            OUTPUT INSERTED.Id AS OrgId
                            VALUES(@OrgId, @Name)";

                var parameters = new DynamicParameters();
                parameters.Add("@OrgId", Guid.NewGuid());
                parameters.Add("@Name", request.OrganizationName);

                var result = await connection.ExecuteScalarAsync<Guid>(query, parameters, commandType: CommandType.Text);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error thrown in OrganizationRepository.RegisterOrganization. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
                throw;
            }
        }

        public async Task<bool> AddOrganizationMember(Guid orgId, Guid userId, string role, CancellationToken cancellationToken)
        {
            try
            {
                var inputParams = new { orgId, userId, role };
                _logger.LogInformation("Information in OrganizationRepository.AddOrganizationMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));

                using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

                var query = @"INSERT INTO OrganizationMembers (UserId, OrgId, [Role])
                            VALUES (@UserId, @OrgId, @Role)";

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@OrgId", orgId);
                parameters.Add("@Role", role);

                var result = await connection.ExecuteAsync(query, parameters, commandType: CommandType.Text);
                return result > 0;
            }
            catch (Exception ex)
            {
                var inputParams = new { orgId, userId, role };
                _logger.LogError(ex, "Error thrown in OrganizationRepository.AddOrganizationMember. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
                throw;
            }
        }
    }
}
