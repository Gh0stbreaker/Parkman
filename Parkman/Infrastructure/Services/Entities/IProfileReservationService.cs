using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IProfileReservationService : IGenericService<ProfileReservation> { }

public class ProfileReservationService : GenericService<ProfileReservation>, IProfileReservationService
{
    public ProfileReservationService(IProfileReservationRepository repository) : base(repository) { }
}
