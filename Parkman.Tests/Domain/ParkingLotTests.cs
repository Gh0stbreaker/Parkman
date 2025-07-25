using System;
using Parkman.Domain.Entities;
using Parkman.Shared.Enums;
using Xunit;

namespace Parkman.Tests.Domain;

public class ParkingLotTests
{
    [Fact]
    public void AddSpot_sets_lot_reference_and_identifier()
    {
        var lot = new ParkingLot("LotA", "Address");
        var spot = new ParkingSpot("1", ParkingSpotType.Regular, ParkingSpotAccessibility.None, ParkingSpotAllowedPropulsionType.Any);
        lot.AddSpot(spot);

        Assert.Equal(lot, spot.ParkingLot);
        Assert.Equal("LotA1", spot.Identifier);
    }

    [Fact]
    public void Update_invalid_name_should_throw()
    {
        var lot = new ParkingLot("LotA", "Address");
        Assert.Throws<ArgumentException>(() => lot.Update("", "Address"));
    }
}
