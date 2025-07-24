using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IParkingLotRepository : IGenericRepository<ParkingLot> { }

public class ParkingLotRepository : GenericRepository<ParkingLot>, IParkingLotRepository
{
    public ParkingLotRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ParkingLot>> logger)
        : base(context, logger) { }
}
