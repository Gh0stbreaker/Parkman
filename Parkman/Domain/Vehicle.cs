using System;

namespace Parkman.Domain;

public class Vehicle
{
    public int Id { get; private set; }

    public string LicensePlate { get; private set; } = string.Empty;
    public string Brand { get; private set; } = string.Empty;
    public string Type { get; private set; } = string.Empty;
    public string PropulsionType { get; private set; } = string.Empty;

    public bool IsShareable { get; private set; }

    public string? CompanyProfileUserId { get; private set; }
    public CompanyProfile? CompanyProfile { get; private set; }

    public string? PersonProfileUserId { get; private set; }
    public PersonProfile? PersonProfile { get; private set; }

    private Vehicle() { }

    public Vehicle(string licensePlate, string brand, string type, string propulsionType, bool isShareable = false)
    {
        Update(licensePlate, brand, type, propulsionType, isShareable);
    }

    public void Update(string licensePlate, string brand, string type, string propulsionType, bool isShareable = false)
    {
        if (string.IsNullOrWhiteSpace(licensePlate))
            throw new ArgumentException("License plate is required", nameof(licensePlate));
        if (string.IsNullOrWhiteSpace(brand))
            throw new ArgumentException("Brand is required", nameof(brand));
        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("Type is required", nameof(type));
        if (string.IsNullOrWhiteSpace(propulsionType))
            throw new ArgumentException("Propulsion type is required", nameof(propulsionType));

        LicensePlate = licensePlate;
        Brand = brand;
        Type = type;
        PropulsionType = propulsionType;
        IsShareable = isShareable;
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
