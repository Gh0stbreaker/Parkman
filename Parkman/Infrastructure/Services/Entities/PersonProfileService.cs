using Parkman.Shared.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class PersonProfileService : GenericService<PersonProfile>, IPersonProfileService
{
    public PersonProfileService(IPersonProfileRepository repository) : base(repository) { }
}
