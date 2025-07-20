using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IPersonProfileRepository : IGenericRepository<PersonProfile> { }

public class PersonProfileRepository : GenericRepository<PersonProfile>, IPersonProfileRepository
{
    public PersonProfileRepository(ApplicationDbContext context) : base(context) { }
}
