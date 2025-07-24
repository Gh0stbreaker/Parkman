using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IReservationRepository : IGenericRepository<Reservation> { }

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<Reservation>> logger)
        : base(context, logger) { }
}
