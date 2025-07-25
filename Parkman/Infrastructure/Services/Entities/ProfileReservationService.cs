using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class ProfileReservationService : GenericService<ProfileReservation>, IProfileReservationService
{
    public ProfileReservationService(IProfileReservationRepository repository) : base(repository) { }
}
