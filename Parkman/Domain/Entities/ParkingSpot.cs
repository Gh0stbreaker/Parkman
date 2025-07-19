using System;
using System.Collections.Generic;

namespace Parkman.Domain.Entities;

public class ParkingSpot
{
    public int Id { get; private set; }

    public string Identifier { get; private set; } = string.Empty;

    public int ParkingLotId { get; private set; }
    public ParkingLot ParkingLot { get; private set; } = null!;

    public List<Reservation> Reservations { get; } = new();

    private ParkingSpot() { }

    public ParkingSpot(string identifier)
    {
        Update(identifier);
    }

    public void Update(string identifier)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier is required", nameof(identifier));

        Identifier = identifier;
    }

    internal void SetParkingLot(ParkingLot lot)
    {
        ParkingLot = lot;
        ParkingLotId = lot.Id;
    }

    internal void AddReservation(Reservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));
        Reservations.Add(reservation);
        reservation.SetParkingSpot(this);
    }
}
