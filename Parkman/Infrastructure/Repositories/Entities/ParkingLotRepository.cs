using Microsoft.Extensions.Logging;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class ParkingLotRepository : GenericRepository<ParkingLot>, IParkingLotRepository
{
    public ParkingLotRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ParkingLot>> logger)
        : base(context, logger) { }
}
