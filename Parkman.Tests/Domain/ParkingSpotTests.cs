using System;
using Parkman.Domain.Entities;
using Parkman.Domain.Enums;
using Xunit;

namespace Parkman.Tests.Domain;

public class ParkingSpotTests
{
    [Fact]
    public void Adding_overlapping_reservation_should_throw()
    {
        var spot = new ParkingSpot("A1", ParkingSpotType.Regular, ParkingSpotAccessibility.None, ParkingSpotAllowedPropulsionType.Any);
        var first = new Reservation(DateTime.Parse("2024-01-01T10:00:00"), DateTime.Parse("2024-01-01T11:00:00"));
        spot.AddReservation(first);

        var overlapping = new Reservation(DateTime.Parse("2024-01-01T10:30:00"), DateTime.Parse("2024-01-01T11:30:00"));

        Assert.Throws<InvalidOperationException>(() => spot.AddReservation(overlapping));
    }

    [Fact]
    public void Non_overlapping_reservation_should_be_added()
    {
        var spot = new ParkingSpot("A1", ParkingSpotType.Regular, ParkingSpotAccessibility.None, ParkingSpotAllowedPropulsionType.Any);
        var first = new Reservation(DateTime.Parse("2024-01-01T10:00:00"), DateTime.Parse("2024-01-01T11:00:00"));
        spot.AddReservation(first);

        var second = new Reservation(DateTime.Parse("2024-01-01T11:00:00"), DateTime.Parse("2024-01-01T12:00:00"));
        spot.AddReservation(second);

        Assert.Equal(2, spot.Reservations.Count);
    }
}
