using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IParkingSpotRepository : IGenericRepository<ParkingSpot> { }

public class ParkingSpotRepository : GenericRepository<ParkingSpot>, IParkingSpotRepository
{
    public ParkingSpotRepository(ApplicationDbContext context) : base(context) { }
}
