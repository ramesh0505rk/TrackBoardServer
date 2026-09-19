using System.Data;

namespace TrackBoard.Infrastructure.Presistence
{
    public interface IDbConnectionFactory
    {
        Task<IDbConnection> GetOpenConnection(CancellationToken cancellationToken);
    }
}
