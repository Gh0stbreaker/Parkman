using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IParkingSpotService : IGenericService<ParkingSpot>
{
    Task<IReadOnlyList<ParkingSpot>> ListAvailableAsync(DateTime startTime, DateTime endTime);
}

