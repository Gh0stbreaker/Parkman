using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ParkingLotService : GenericService<ParkingLot>, IParkingLotService
{
    public ParkingLotService(IParkingLotRepository repository) : base(repository) { }
}
