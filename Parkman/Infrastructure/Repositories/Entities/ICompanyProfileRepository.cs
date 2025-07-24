using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface ICompanyProfileRepository : IGenericRepository<CompanyProfile> { }

public class CompanyProfileRepository : GenericRepository<CompanyProfile>, ICompanyProfileRepository
{
    public CompanyProfileRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<CompanyProfile>> logger)
        : base(context, logger) { }
}
