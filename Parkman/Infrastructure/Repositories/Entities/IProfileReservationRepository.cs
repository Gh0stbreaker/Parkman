using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IProfileReservationRepository : IGenericRepository<ProfileReservation> { }

public class ProfileReservationRepository : GenericRepository<ProfileReservation>, IProfileReservationRepository
{
    public ProfileReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ProfileReservation>> logger)
        : base(context, logger) { }
}
