using Parkman.Shared.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ParkingLotService : GenericService<ParkingLot>, IParkingLotService
{
    public ParkingLotService(IParkingLotRepository repository) : base(repository) { }
}
