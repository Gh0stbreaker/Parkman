using Microsoft.Extensions.Logging;
using Parkman.Shared.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<Reservation>> logger)
        : base(context, logger) { }
}
