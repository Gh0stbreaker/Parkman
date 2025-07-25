using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class PersonProfileService : GenericService<PersonProfile>, IPersonProfileService
{
    public PersonProfileService(IPersonProfileRepository repository) : base(repository) { }
}
