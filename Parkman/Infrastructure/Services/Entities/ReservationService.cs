using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ReservationService : GenericService<Reservation>, IReservationService
{
    public ReservationService(IReservationRepository repository) : base(repository) { }
}
