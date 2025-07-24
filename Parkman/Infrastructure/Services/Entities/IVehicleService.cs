using Parkman.Shared.Entities;
using System.Threading.Tasks;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IVehicleService : IGenericService<Vehicle>
{
    Task<bool> LicensePlateExistsAsync(string licensePlate);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
}

