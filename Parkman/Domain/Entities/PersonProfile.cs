using System;
using System.Collections.Generic;

namespace Parkman.Domain.Entities;

public class PersonProfile
{
    public string UserId { get; private set; } = null!;
    public ApplicationUser User { get; private set; } = null!;

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public DateOnly? DateOfBirth { get; private set; }
    public string PhoneNumber { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    public Vehicle? Vehicle { get; private set; }

    private readonly List<ProfileReservation> _profileReservations = new();
    public IReadOnlyCollection<ProfileReservation> ProfileReservations => _profileReservations;

    public string? CompanyProfileUserId { get; private set; }
    public CompanyProfile? CompanyProfile { get; private set; }

    public bool IsCompanyApproved { get; private set; }

    private PersonProfile() { }

    public PersonProfile(
        string firstName,
        string lastName,
        DateOnly? dateOfBirth,
        string phoneNumber,
        string address)
    {
        Update(firstName, lastName, dateOfBirth, phoneNumber, address);
    }

    public void Update(
        string firstName,
        string lastName,
        DateOnly? dateOfBirth,
        string phoneNumber,
        string address)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name is required", nameof(firstName));
        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name is required", nameof(lastName));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        Address = address;
    }

    internal void SetVehicle(Vehicle vehicle)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
        if (Vehicle != null)
        {
            throw new InvalidOperationException("Person profile already has a vehicle.");
        }
        if (vehicle.CompanyProfileUserId != null && vehicle.CompanyProfileUserId != CompanyProfileUserId)
        {
            throw new InvalidOperationException("Person is not authorized to use this company vehicle.");
        }
        Vehicle = vehicle;
        vehicle.SetPersonProfile(this);
    }

    internal void AddReservation(Reservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));
        var link = new ProfileReservation(this, reservation);
        _profileReservations.Add(link);
        reservation.AddProfileReservation(link);
    }

    internal void SetUser(ApplicationUser user)
    {
        User = user;
        UserId = user.Id;
    }

    internal void SetCompanyProfile(CompanyProfile companyProfile)
    {
        CompanyProfile = companyProfile;
        CompanyProfileUserId = companyProfile.UserId;
    }

    internal void ApproveCompanyMembership()
    {
        IsCompanyApproved = true;
    }
}
