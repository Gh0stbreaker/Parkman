using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ParkingSpotService : GenericService<ParkingSpot>, IParkingSpotService
{
    public ParkingSpotService(IParkingSpotRepository repository) : base(repository) { }
}
