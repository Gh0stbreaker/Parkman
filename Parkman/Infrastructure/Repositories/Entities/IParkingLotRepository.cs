using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IParkingLotRepository : IGenericRepository<ParkingLot> { }

public class ParkingLotRepository : GenericRepository<ParkingLot>, IParkingLotRepository
{
    public ParkingLotRepository(ApplicationDbContext context) : base(context) { }
}
