using Microsoft.Extensions.Logging;
using Parkman.Shared.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class ParkingSpotRepository : GenericRepository<ParkingSpot>, IParkingSpotRepository
{
    public ParkingSpotRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ParkingSpot>> logger)
        : base(context, logger) { }
}
