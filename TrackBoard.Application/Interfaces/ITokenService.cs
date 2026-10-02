using TrackBoard.Domain.Entities;

namespace TrackBoard.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
