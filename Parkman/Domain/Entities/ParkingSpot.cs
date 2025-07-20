using System;
using System.Collections.Generic;
using System.Linq;
using Parkman.Domain.Enums;

namespace Parkman.Domain.Entities;

public class ParkingSpot
{
    public int Id { get; private set; }

    public string Label { get; private set; } = string.Empty;

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
        string label,
        ParkingSpotType type,
        ParkingSpotAccessibility accessibility,
        ParkingSpotAllowedPropulsionType allowedPropulsion)
    {
        Update(label, type, accessibility, allowedPropulsion);
    }

    public void Update(
        string label,
        ParkingSpotType type,
        ParkingSpotAccessibility accessibility,
        ParkingSpotAllowedPropulsionType allowedPropulsion)
    {
        if (string.IsNullOrWhiteSpace(label))
            throw new ArgumentException("Label is required", nameof(label));

        Label = label;
        RebuildIdentifier();
        Type = type;
        Accessibility = accessibility;
        AllowedPropulsion = allowedPropulsion;
    }

    internal void SetParkingLot(ParkingLot lot)
    {
        ParkingLot = lot;
        ParkingLotId = lot.Id;
        RebuildIdentifier();
    }

    public void AddReservation(Reservation reservation)
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

    private void RebuildIdentifier()
    {
        Identifier = ParkingLot != null ? $"{ParkingLot.Name}{Label}" : Label;
    }
}
