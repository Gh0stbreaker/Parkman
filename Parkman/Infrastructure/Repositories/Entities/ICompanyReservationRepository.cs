using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface ICompanyReservationRepository : IGenericRepository<CompanyReservation> { }

public class CompanyReservationRepository : GenericRepository<CompanyReservation>, ICompanyReservationRepository
{
    public CompanyReservationRepository(ApplicationDbContext context) : base(context) { }
}
