using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using TrackBoard.Application.Interfaces;
using TrackBoard.Application.ResponseDTOs;
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

        }
    }
}
