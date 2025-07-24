using Parkman.Shared.Entities;
using System.Threading.Tasks;

namespace Parkman.Infrastructure.Repositories.Entities;

public interface IVehicleRepository : IGenericRepository<Vehicle>
{
    Task<bool> LicensePlateExistsAsync(string licensePlate);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
}

