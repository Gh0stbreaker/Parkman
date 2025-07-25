using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IParkingSpotRepository : IGenericRepository<ParkingSpot>
{
    Task<IReadOnlyList<ParkingSpot>> ListAvailableAsync(DateTime startTime, DateTime endTime);
}

