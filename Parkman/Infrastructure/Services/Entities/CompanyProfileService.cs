using Parkman.Shared.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class CompanyProfileService : GenericService<CompanyProfile>, ICompanyProfileService
{
    public CompanyProfileService(ICompanyProfileRepository repository) : base(repository) { }
}
