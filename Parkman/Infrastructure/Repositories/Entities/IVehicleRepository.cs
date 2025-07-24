using Parkman.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IVehicleRepository : IGenericRepository<Vehicle> { }

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(
        ApplicationDbContext context,
        ILogger<GenericRepository<Vehicle>> logger)
        : base(context, logger) { }
}
