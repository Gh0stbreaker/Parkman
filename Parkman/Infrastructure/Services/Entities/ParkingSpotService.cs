using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ParkingSpotService : GenericService<ParkingSpot>, IParkingSpotService
{
    private readonly IParkingSpotRepository _repository;

    public ParkingSpotService(IParkingSpotRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<IReadOnlyList<ParkingSpot>> ListAvailableAsync(DateTime startTime, DateTime endTime)
    {
        return _repository.ListAvailableAsync(startTime, endTime);
    }
}
