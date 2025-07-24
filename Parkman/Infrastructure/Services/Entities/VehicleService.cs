using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class VehicleService : GenericService<Vehicle>, IVehicleService
{
    public VehicleService(IVehicleRepository repository) : base(repository) { }
}
