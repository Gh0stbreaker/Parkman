using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface ICompanyProfileService : IGenericService<CompanyProfile> { }

public class CompanyProfileService : GenericService<CompanyProfile>, ICompanyProfileService
{
    public CompanyProfileService(ICompanyProfileRepository repository) : base(repository) { }
}
