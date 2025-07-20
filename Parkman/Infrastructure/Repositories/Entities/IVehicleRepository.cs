using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IVehicleRepository : IGenericRepository<Vehicle> { }

public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(ApplicationDbContext context) : base(context) { }
}
