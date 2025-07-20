using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IReservationRepository : IGenericRepository<Reservation> { }

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context) { }
}
