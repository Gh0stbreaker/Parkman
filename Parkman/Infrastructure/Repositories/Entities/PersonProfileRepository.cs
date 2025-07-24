using Microsoft.Extensions.Logging;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public class PersonProfileRepository : GenericRepository<PersonProfile>, IPersonProfileRepository
{
    public PersonProfileRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<PersonProfile>> logger)
        : base(context, logger) { }
}
