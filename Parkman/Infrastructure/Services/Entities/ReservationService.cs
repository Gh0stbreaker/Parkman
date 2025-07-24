using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ReservationService : GenericService<Reservation>, IReservationService
{
    public ReservationService(IReservationRepository repository) : base(repository) { }
}
