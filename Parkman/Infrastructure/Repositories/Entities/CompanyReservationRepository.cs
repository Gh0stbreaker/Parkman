using Microsoft.Extensions.Logging;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class CompanyReservationRepository : GenericRepository<CompanyReservation>, ICompanyReservationRepository
{
    public CompanyReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<CompanyReservation>> logger)
        : base(context, logger) { }
}
