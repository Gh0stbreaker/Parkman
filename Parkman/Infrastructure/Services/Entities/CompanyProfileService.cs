using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class CompanyProfileService : GenericService<CompanyProfile>, ICompanyProfileService
{
    public CompanyProfileService(ICompanyProfileRepository repository) : base(repository) { }
}
