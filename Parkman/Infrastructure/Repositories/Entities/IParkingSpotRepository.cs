using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IParkingSpotRepository : IGenericRepository<ParkingSpot> { }

public class ParkingSpotRepository : GenericRepository<ParkingSpot>, IParkingSpotRepository
{
    public ParkingSpotRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ParkingSpot>> logger)
        : base(context, logger) { }
}
