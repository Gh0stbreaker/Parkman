using Microsoft.Extensions.Logging;
using Parkman.Shared.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class ProfileReservationRepository : GenericRepository<ProfileReservation>, IProfileReservationRepository
{
    public ProfileReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<ProfileReservation>> logger)
        : base(context, logger) { }
}
