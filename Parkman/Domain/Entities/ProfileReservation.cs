using System;

namespace Parkman.Shared.Entities;

public class ProfileReservation
{
    public string PersonProfileUserId { get; private set; } = null!;
    public PersonProfile PersonProfile { get; private set; } = null!;

    public int ReservationId { get; private set; }
    public Reservation Reservation { get; private set; } = null!;

    private ProfileReservation() { }

    internal ProfileReservation(PersonProfile profile, Reservation reservation)
    {
        SetPersonProfile(profile);
        SetReservation(reservation);
    }

    internal void SetPersonProfile(PersonProfile profile)
    {
        PersonProfile = profile ?? throw new ArgumentNullException(nameof(profile));
        PersonProfileUserId = profile.UserId;
    }

    internal void SetReservation(Reservation reservation)
    {
        Reservation = reservation ?? throw new ArgumentNullException(nameof(reservation));
        ReservationId = reservation.Id;
    }
}
