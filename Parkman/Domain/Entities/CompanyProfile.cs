using System;

namespace Parkman.Domain.Entities;

public class CompanyProfile
{
    public string UserId { get; private set; } = null!;
    public ApplicationUser User { get; private set; } = null!;

    public string CompanyName { get; private set; } = string.Empty;
    public string Ico { get; private set; } = string.Empty;
    public string Dic { get; private set; } = string.Empty;
    public string ContactPersonName { get; private set; } = string.Empty;
    public string ContactEmail { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string BillingAddress { get; private set; } = string.Empty;

    public List<Vehicle> Vehicles { get; } = new();
    public List<PersonProfile> Members { get; } = new();
    public List<CompanyReservation> CompanyReservations { get; } = new();

    private CompanyProfile() { }

    public CompanyProfile(
        string companyName,
        string ico,
        string dic,
        string contactPersonName,
        string contactEmail,
        string phoneNumber,
        string billingAddress)
    {
        Update(companyName, ico, dic, contactPersonName, contactEmail, phoneNumber, billingAddress);
    }

    public void Update(
        string companyName,
        string ico,
        string dic,
        string contactPersonName,
        string contactEmail,
        string phoneNumber,
        string billingAddress)
    {
        if (string.IsNullOrWhiteSpace(companyName))
            throw new ArgumentException("Company name is required", nameof(companyName));
        if (string.IsNullOrWhiteSpace(ico))
            throw new ArgumentException("ICO is required", nameof(ico));
        if (string.IsNullOrWhiteSpace(dic))
            throw new ArgumentException("DIC is required", nameof(dic));
        if (string.IsNullOrWhiteSpace(contactPersonName))
            throw new ArgumentException("Contact person name is required", nameof(contactPersonName));
        if (string.IsNullOrWhiteSpace(contactEmail))
            throw new ArgumentException("Contact email is required", nameof(contactEmail));
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number is required", nameof(phoneNumber));
        if (string.IsNullOrWhiteSpace(billingAddress))
            throw new ArgumentException("Billing address is required", nameof(billingAddress));

        CompanyName = companyName;
        Ico = ico;
        Dic = dic;
        ContactPersonName = contactPersonName;
        ContactEmail = contactEmail;
        PhoneNumber = phoneNumber;
        BillingAddress = billingAddress;
    }

    internal void AddVehicle(Vehicle vehicle)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));
        Vehicles.Add(vehicle);
        vehicle.SetCompanyProfile(this);
    }

    internal void AddMember(PersonProfile personProfile)
    {
        if (personProfile == null) throw new ArgumentNullException(nameof(personProfile));
        Members.Add(personProfile);
        personProfile.SetCompanyProfile(this);
    }

    internal void AddReservation(Reservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));
        var link = new CompanyReservation(this, reservation);
        CompanyReservations.Add(link);
        reservation.AddCompanyReservation(link);
    }

    internal void SetUser(ApplicationUser user)
    {
        User = user;
        UserId = user.Id;
    }
}
