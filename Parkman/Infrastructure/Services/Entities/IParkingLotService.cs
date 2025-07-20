using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IParkingLotService : IGenericService<ParkingLot> { }

public class ParkingLotService : GenericService<ParkingLot>, IParkingLotService
{
    public ParkingLotService(IParkingLotRepository repository) : base(repository) { }
}
