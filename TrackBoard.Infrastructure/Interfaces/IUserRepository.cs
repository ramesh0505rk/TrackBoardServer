using TrackBoard.Domain.Entities;

namespace TrackBoard.Infrastructure.Interfaces
{
	public interface IUserRepository
	{
		Task<bool> CheckUserExists(string email, CancellationToken cancellationToken);
		Task<User> CreateUser(SignUpRequest request, CancellationToken cancellationToken);
		Task<Guid?> ValidateUser(string userName, string password, CancellationToken cancellationToken);
		Task<User> GetUserDetailsByUserId(Guid? userId, CancellationToken cancellationToken);
		Task<bool> UserNameExists(string UserName, CancellationToken cancellationToken);
	}
}
