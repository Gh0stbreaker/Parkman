using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IPersonProfileRepository : IGenericRepository<PersonProfile> { }

public class PersonProfileRepository : GenericRepository<PersonProfile>, IPersonProfileRepository
{
    public PersonProfileRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<PersonProfile>> logger)
        : base(context, logger) { }
}
