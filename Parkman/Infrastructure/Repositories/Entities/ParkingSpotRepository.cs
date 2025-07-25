using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class ParkingSpotRepository : GenericRepository<ParkingSpot>, IParkingSpotRepository
{
    public ParkingSpotRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ParkingSpot>> logger)
        : base(context, logger) { }

    public async Task<IReadOnlyList<ParkingSpot>> ListAvailableAsync(DateTime startTime, DateTime endTime)
    {
        return await DbSet
            .Include(s => s.ParkingLot)
            .Where(s => !s.Reservations.Any(r => r.StartTime < endTime && startTime < r.EndTime))
            .ToListAsync();
    }
}
