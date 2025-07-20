using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface ICompanyProfileRepository : IGenericRepository<CompanyProfile> { }

public class CompanyProfileRepository : GenericRepository<CompanyProfile>, ICompanyProfileRepository
{
    public CompanyProfileRepository(ApplicationDbContext context) : base(context) { }
}
