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
                _logger.LogInformation("Information in UserRepository.CheckUserExists. Input parameters: {orgName}", orgName);

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

        public async Task<Guid> RegisterOrganization(OrganizationRegisterRequest request, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error thrown in OrganizationRepository.RegisterOrganization. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
                throw;
            }
        }
    }
}
