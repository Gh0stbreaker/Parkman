using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IProfileReservationRepository : IGenericRepository<ProfileReservation> { }

public class ProfileReservationRepository : GenericRepository<ProfileReservation>, IProfileReservationRepository
{
    public ProfileReservationRepository(ApplicationDbContext context) : base(context) { }
}
