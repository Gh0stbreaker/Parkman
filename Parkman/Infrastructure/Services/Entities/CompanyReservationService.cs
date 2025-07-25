using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class CompanyReservationService : GenericService<CompanyReservation>, ICompanyReservationService
{
    public CompanyReservationService(ICompanyReservationRepository repository) : base(repository) { }
}
