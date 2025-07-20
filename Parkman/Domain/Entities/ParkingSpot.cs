using System;
using System.Collections.Generic;
using System.Linq;
using Parkman.Domain.Enums;

namespace Parkman.Domain.Entities;

public class ParkingSpot
{
    public int Id { get; private set; }

    public string Identifier { get; private set; } = string.Empty;

    public ParkingSpotType Type { get; private set; }
    public ParkingSpotAccessibility Accessibility { get; private set; }
    public ParkingSpotAllowedPropulsionType AllowedPropulsion { get; private set; }

    public int ParkingLotId { get; private set; }
    public ParkingLot ParkingLot { get; private set; } = null!;

    private readonly List<Reservation> _reservations = new();
    public IReadOnlyCollection<Reservation> Reservations => _reservations;

    private ParkingSpot() { }

    public ParkingSpot(
        string identifier,
        ParkingSpotType type,
        ParkingSpotAccessibility accessibility,
        ParkingSpotAllowedPropulsionType allowedPropulsion)
    {
        Update(identifier, type, accessibility, allowedPropulsion);
    }

    public void Update(
        string identifier,
        ParkingSpotType type,
        ParkingSpotAccessibility accessibility,
        ParkingSpotAllowedPropulsionType allowedPropulsion)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier is required", nameof(identifier));

        Identifier = identifier;
        Type = type;
        Accessibility = accessibility;
        AllowedPropulsion = allowedPropulsion;
    }

    internal void SetParkingLot(ParkingLot lot)
    {
        ParkingLot = lot;
        ParkingLotId = lot.Id;
    }

    internal void AddReservation(Reservation reservation)
    {
        if (reservation == null) throw new ArgumentNullException(nameof(reservation));
        if (HasReservationConflict(reservation.StartTime, reservation.EndTime))
        {
            throw new InvalidOperationException("Reservation overlaps with an existing reservation.");
        }

        _reservations.Add(reservation);
        reservation.SetParkingSpot(this);
    }

    public bool HasReservationConflict(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time", nameof(endTime));

        return _reservations.Any(r => r.StartTime < endTime && startTime < r.EndTime);
    }
}
