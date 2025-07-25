using System;
using Parkman.Domain.Entities;
using Parkman.Shared.Enums;
using Xunit;

namespace Parkman.Tests.Domain;

public class VehicleTests
{
    [Fact]
    public void Update_invalid_license_plate_should_throw()
    {
        var vehicle = new Vehicle("AAA111", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric);
        Assert.Throws<ArgumentException>(() => vehicle.Update("", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric));
    }

    [Fact]
    public void Update_invalid_brand_should_throw()
    {
        var vehicle = new Vehicle("AAA111", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric);
        Assert.Throws<ArgumentException>(() => vehicle.Update("BBB222", (VehicleBrand)999, VehicleType.Car, VehiclePropulsionType.Electric));
    }

    [Fact]
    public void Update_valid_changes_properties()
    {
        var vehicle = new Vehicle("AAA111", VehicleBrand.Tesla, VehicleType.Car, VehiclePropulsionType.Electric);
        vehicle.Update("BBB222", VehicleBrand.Ford, VehicleType.Truck, VehiclePropulsionType.Diesel, true, "pwd");

        Assert.Equal("BBB222", vehicle.LicensePlate);
        Assert.Equal(VehicleBrand.Ford, vehicle.Brand);
        Assert.Equal(VehicleType.Truck, vehicle.Type);
        Assert.Equal(VehiclePropulsionType.Diesel, vehicle.PropulsionType);
        Assert.True(vehicle.IsShareable);
        Assert.Equal("pwd", vehicle.PairingPassword);
    }
}
