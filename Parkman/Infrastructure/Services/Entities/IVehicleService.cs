using System.Threading.Tasks;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Domain.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public interface IVehicleService : IGenericService<Vehicle>
{
    Task<bool> LicensePlateExistsAsync(string licensePlate);
    Task<Vehicle?> GetByLicensePlateAsync(string licensePlate);
}

