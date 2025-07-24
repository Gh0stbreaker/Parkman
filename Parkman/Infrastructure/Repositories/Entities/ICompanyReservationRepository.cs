using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface ICompanyReservationRepository : IGenericRepository<CompanyReservation> { }

public class CompanyReservationRepository : GenericRepository<CompanyReservation>, ICompanyReservationRepository
{
    public CompanyReservationRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<CompanyReservation>> logger)
        : base(context, logger) { }
}
