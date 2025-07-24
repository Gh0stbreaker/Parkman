using Parkman.Domain.Entities;
using Parkman.Infrastructure.Repositories.Entities;

namespace Parkman.Infrastructure.Services.Entities;

public class VehicleService : GenericService<Vehicle>, IVehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(IVehicleRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public Task<bool> LicensePlateExistsAsync(string licensePlate)
    {
        return _repository.LicensePlateExistsAsync(licensePlate);
    }

    public Task<Vehicle?> GetByLicensePlateAsync(string licensePlate)
    {
        return _repository.GetByLicensePlateAsync(licensePlate);
    }
}
