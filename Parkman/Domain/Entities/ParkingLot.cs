using System;
using System.Collections.Generic;

namespace Parkman.Domain.Entities;

public class ParkingLot
{
    public int Id { get; private set; }

    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;

    private readonly List<ParkingSpot> _spots = new();
    public IReadOnlyCollection<ParkingSpot> Spots => _spots;

    private ParkingLot() { }

    public ParkingLot(string name, string address)
    {
        Update(name, address);
    }

    public void Update(string name, string address)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address is required", nameof(address));

        Name = name;
        Address = address;
    }

    internal void AddSpot(ParkingSpot spot)
    {
        if (spot == null) throw new ArgumentNullException(nameof(spot));
        _spots.Add(spot);
        spot.SetParkingLot(this);
    }
}
