using Dapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TrackBoard.Domain.Entities;
using TrackBoard.Infrastructure.Interfaces;
using TrackBoard.Infrastructure.Presistence;

namespace TrackBoard.Infrastructure.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly IDbConnectionFactory _dbConnectionFactory;
		private readonly ILogger<UserRepository> _logger;
		private readonly IPasswordHasher _passwordHasher;

		public UserRepository(IDbConnectionFactory dbConnectionFactory, ILogger<UserRepository> logger, IPasswordHasher passwordHasher)
		{
			_dbConnectionFactory = dbConnectionFactory;
			_logger = logger;
			_passwordHasher = passwordHasher;
		}

		public async Task<bool> CheckUserExists(string email, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Information in UserRepository.CheckUserExists. Input parameters: {userEmail}", email);
				using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

				var query = "SELECT COUNT(1) FROM Users WHERE Email = @Email";

				var parameters = new DynamicParameters();
				parameters.Add("@Email", email);

				var result = await connection.QueryFirstOrDefaultAsync<int>(query, parameters, commandType: System.Data.CommandType.Text);

				return result > 0;
			}
			catch (Exception ex)
			{
				var inputParams = new { email };
				_logger.LogError(ex, "Error thrown in UserRepository.CheckUserExists. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
				throw;
			}
		}

		public async Task<User> CreateUser(SignUpRequest request, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Information in UserRepository.CreateUser. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
				using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

				var hashedPassword = _passwordHasher.HashPassword(request.Password);

				var query = @"INSERT INTO Users (Id, UserName, FirstName, LastName, Email, Password) 
                             OUTPUT INSERTED.Id AS UserId, 
                                    INSERTED.UserName, 
                                    INSERTED.FirstName, 
                                    INSERTED.LastName, 
                                    INSERTED.Email, 
                                    INSERTED.Password 
                             VALUES (@UserId, @UserName, @FirstName, @LastName, @Email, @Password)";

				var parameters = new DynamicParameters();
				parameters.Add("@UserId", Guid.NewGuid());
				parameters.Add("@UserName", request.UserName);
				parameters.Add("@FirstName", request.FirstName);
				parameters.Add("@LastName", request.LastName);
				parameters.Add("@Email", request.Email);
				parameters.Add("@Password", hashedPassword);

				var result = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);
				result.OrgId = null;
				return result;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserRepository.CreateUser. Input parameters: {InputParams}", JsonConvert.SerializeObject(request));
				throw;
			}
		}

		public async Task<Guid?> ValidateUser(string userName, string password, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Information in UserRepository.ValidateUser. Input parameters: UserName = {UserName}", userName);
				using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

				var query = @"SELECT Id, [Password] FROM TrackBoard.dbo.Users WHERE UserName COLLATE Latin1_General_CS_AS = @UserName OR Email COLLATE Latin1_General_CS_AS = @UserName";

				var parameters = new DynamicParameters();
				parameters.Add("@UserName", userName);

				var user = await connection.QueryFirstOrDefaultAsync<(Guid? Id, string Password)>(query, parameters);

				if (user.Id == null || string.IsNullOrEmpty(user.Password))
				{
					return null;
				}

				var isPasswordValid = _passwordHasher.VerifyPassword(password, user.Password);
				return isPasswordValid ? user.Id : null;
			}
			catch (Exception ex)
			{
				var inputParams = new { userName };
				_logger.LogError(ex, "Error thrown in UserRepository.ValidateUser. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
				throw;
			}
		}

		public async Task<User> GetUserDetailsByUserId(Guid? userId, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogInformation("Information in UserRepository.GetUserDetailsByUserId. Input parameters: UserId = {UserId}", userId);
				using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);

				// Get user details by userId
				var query = @"SELECT Id AS UserId, UserName, FirstName, LastName, Email 
                            FROM Users WHERE Id = @UserId";

				var parameters = new DynamicParameters();
				parameters.Add("@UserId", userId);

				var result = await connection.QueryFirstOrDefaultAsync<User>(query, parameters);

				// Get the user's orgId by userId
				var getOrgId = @"SELECT OrgId FROM dbo.OrganizationMembers WHERE UserId = @UserId";

				var getOrgIdParams = new DynamicParameters();
				getOrgIdParams.Add("@UserId", userId);

				var orgId = await connection.ExecuteScalarAsync<Guid?>(getOrgId, getOrgIdParams);
				result.OrgId = orgId;

				return result;
			}
			catch (Exception ex)
			{
				var inputParams = new { userId };
				_logger.LogError(ex, "Error thrown in UserRepository.GetUserDetailsByUserId. Input parameters: {InputParams}", JsonConvert.SerializeObject(inputParams));
				throw;
			}
		}

		public async Task<bool> UserNameExists(string UserName, CancellationToken cancellationToken)
		{
			try
			{
				using var connection = await _dbConnectionFactory.GetOpenConnection(cancellationToken);
				var query = @"SELECT COUNT(1) FROM [Users] WHERE UserName COLLATE Latin1_General_CS_AS = @UserName";

				var parameters = new DynamicParameters();
				parameters.Add("@UserName", UserName);

				var result = await connection.ExecuteScalarAsync<int>(query, parameters, commandType: System.Data.CommandType.Text);
				return result > 0;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserRepository.GetUserDetailsByUserId. Input parameters: {InputParams}", JsonConvert.SerializeObject(new { UserName }));
				throw;
			}
		}
	}
}
