using Microsoft.Extensions.Logging;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class CompanyProfileRepository : GenericRepository<CompanyProfile>, ICompanyProfileRepository
{
    public CompanyProfileRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<CompanyProfile>> logger)
        : base(context, logger) { }
}
