using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ParkingSpotService : GenericService<ParkingSpot>, IParkingSpotService
{
    public ParkingSpotService(IParkingSpotRepository repository) : base(repository) { }
}
