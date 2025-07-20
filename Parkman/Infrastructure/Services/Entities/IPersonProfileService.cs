using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IPersonProfileService : IGenericService<PersonProfile> { }

public class PersonProfileService : GenericService<PersonProfile>, IPersonProfileService
{
    public PersonProfileService(IPersonProfileRepository repository) : base(repository) { }
}
