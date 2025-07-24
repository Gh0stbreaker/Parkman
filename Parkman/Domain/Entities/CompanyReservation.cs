using System;

namespace Parkman.Shared.Entities;

public class CompanyReservation
{
    public string CompanyProfileUserId { get; private set; } = null!;
    public CompanyProfile CompanyProfile { get; private set; } = null!;

    public int ReservationId { get; private set; }
    public Reservation Reservation { get; private set; } = null!;

    private CompanyReservation() { }

    internal CompanyReservation(CompanyProfile companyProfile, Reservation reservation)
    {
        SetCompanyProfile(companyProfile);
        SetReservation(reservation);
    }

    internal void SetCompanyProfile(CompanyProfile companyProfile)
    {
        CompanyProfile = companyProfile ?? throw new ArgumentNullException(nameof(companyProfile));
        CompanyProfileUserId = companyProfile.UserId;
    }

    internal void SetReservation(Reservation reservation)
    {
        Reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
        ReservationId = reservation.Id;
    }
}
