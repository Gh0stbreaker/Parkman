using System;

using Parkman.Shared.Enums;

namespace Parkman.Shared.Entities;

public class Vehicle
{
    public int Id { get; private set; }

    public string LicensePlate { get; private set; } = string.Empty;
    public VehicleBrand Brand { get; private set; }
    public VehicleType Type { get; private set; }
    public VehiclePropulsionType PropulsionType { get; private set; }

    public bool IsShareable { get; private set; }

    public string? PairingPassword { get; private set; }

    public string? CompanyProfileUserId { get; private set; }
    public CompanyProfile? CompanyProfile { get; private set; }

    public string? PersonProfileUserId { get; private set; }
    public PersonProfile? PersonProfile { get; private set; }

    private Vehicle() { }

    public Vehicle(string licensePlate, VehicleBrand brand, VehicleType type, VehiclePropulsionType propulsionType, bool isShareable = false, string? pairingPassword = null)
    {
        Update(licensePlate, brand, type, propulsionType, isShareable, pairingPassword);
    }

    public void Update(string licensePlate, VehicleBrand brand, VehicleType type, VehiclePropulsionType propulsionType, bool isShareable = false, string? pairingPassword = null)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("License plate is required", nameof(licensePlate));
        if (!Enum.IsDefined(typeof(VehicleBrand), brand))
            throw new ArgumentException("Invalid brand", nameof(brand));
        if (!Enum.IsDefined(typeof(VehicleType), type))
            throw new ArgumentException("Invalid vehicle type", nameof(type));
        if (!Enum.IsDefined(typeof(VehiclePropulsionType), propulsionType))
            throw new ArgumentException("Invalid propulsion type", nameof(propulsionType));

        LicensePlate = licensePlate;
        Brand = brand;
        Type = type;
        PropulsionType = propulsionType;
        IsShareable = isShareable;
        PairingPassword = pairingPassword;
    }

    internal void SetCompanyProfile(CompanyProfile companyProfile)
    {
        CompanyProfile = companyProfile;
        CompanyProfileUserId = companyProfile.UserId;
    }

    internal void SetPersonProfile(PersonProfile personProfile)
    {
        PersonProfile = personProfile;
        PersonProfileUserId = personProfile.UserId;
    }
}
