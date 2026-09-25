using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrackBoard.Application.Interfaces;
using TrackBoard.Application.ResponseDTOs;
using TrackBoard.Domain.Common.ExceptionHandling;
using TrackBoard.Domain.Entities;
using TrackBoard.Infrastructure.Interfaces;

namespace TrackBoard.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly ILogger _logger;
		private readonly IConfiguration _configuration;

		public UserService(IUserRepository userRepository, ILogger<UserService> logger, IConfiguration configuration)
		{
			_userRepository = userRepository;
			_logger = logger;
			_configuration = configuration;
		}
		public async Task<SignInDTO> SignIn(SignInRequest request, CancellationToken cancellationToken)
		{
			var userId = await ValidateUser(request, cancellationToken);
			var user = await GetUserDetailsByUserId(userId, cancellationToken);

			var accessToken = GenerateToken(user);
			return CreateSignInSuccessResponse(accessToken);
		}

		public async Task<SignUpDTO> SignUp(SignUpRequest request, CancellationToken cancellationToken)
		{
			await CheckUserExists(request, cancellationToken);
			var user = await CreateUser(request, cancellationToken);

			var accessToken = GenerateToken(user);
			return CreateSignUpSuccessResponse(accessToken);
		}

		private async Task<User> CreateUser(SignUpRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var user = await _userRepository.CreateUser(request, cancellationToken);
				return user;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserService.CreateUser. Input parameters: {InputParams}", request);
				throw;
			}
		}

		private async Task CheckUserExists(SignUpRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var userExists = await _userRepository.CheckUserExists(request.Email, cancellationToken);
				if (userExists)
				{
					_logger.LogError("User with email {Email} already exists. Input parameters: {InputParams}", request.Email, request);
					throw new BadRequestCustomException(["User with this email already exists."]);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserService.CheckUserExists. Input parameters: {InputParams}", request);
				throw;
			}
		}

		private async Task<Guid?> ValidateUser(SignInRequest request, CancellationToken cancellationToken)
		{
			try
			{
				var userId = await _userRepository.ValidateUser(request.UserName, request.Password, cancellationToken);
				if (userId == null)
				{
					_logger.LogError("Invalid credentials provided for user: {UserName}", request.UserName);
					throw new BadRequestCustomException(["Invalid username or password."]);
				}
				return userId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserService.ValidateUser. Input parameters: {InputParams}", request);
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

		public async Task<bool> UserNameExists(string UserName, CancellationToken cancellationToken)
		{
			try
			{
				var userNameExists = await _userRepository.UserNameExists(UserName, cancellationToken);
				return userNameExists;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error thrown in UserService.UserNameExists. Input parameters: {InputParams}", JsonConvert.SerializeObject(new { UserName }));
				throw;
			}
		}

		private string GenerateToken(User user)
		{
			var jwtSettings = _configuration.GetSection("Jwt");
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim("UserId",user.UserId.ToString()),
				new Claim("OrgId",user.OrgId?.ToString() ?? ""),
				new Claim("UserName",user.UserName),
				new Claim("FirstName",user.FirstName),
				new Claim("LastName",user.LastName),
				new Claim("Email",user.Email)
			};

			var token = new JwtSecurityToken(
				jwtSettings["Issuer"],
				jwtSettings["Audience"],
				claims,
				expires: DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpireInMinutes"])),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		private SignInDTO CreateSignInSuccessResponse(string accessToken)
		{
			return new SignInDTO
			{
				AccessToken = accessToken,
				RequestId = Guid.NewGuid().ToString(),
				ResponseMessage = "Sign-in was successful"
			};
		}

		private SignUpDTO CreateSignUpSuccessResponse(string accessToken)
		{
			return new SignUpDTO
			{
				AccessToken = accessToken,
				RequestId = Guid.NewGuid().ToString(),
				ResponseMessage = "User registered successfully"
			};
		}
	}
}
