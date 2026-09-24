using System;
using System.Collections.Generic;
using System.Text;
using TrackBoard.Application.ResponseDTOs;
using TrackBoard.Domain.Entities;

namespace TrackBoard.Application.Interfaces
{
	public interface IUserService
	{
		Task<SignInDTO> SignIn(SignInRequest request, CancellationToken cancellationToken);
		Task<SignUpDTO> SignUp(SignUpRequest request, CancellationToken cancellationToken);
		Task<bool> UserNameExists(string UserName, CancellationToken cancellationToken);
	}
}
