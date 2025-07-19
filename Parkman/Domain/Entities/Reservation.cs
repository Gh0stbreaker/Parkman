using System;

namespace Parkman.Domain.Entities;

public class Reservation
{
    public int Id { get; private set; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public int ParkingSpotId { get; private set; }
    public ParkingSpot ParkingSpot { get; private set; } = null!;

    private Reservation() { }

    public Reservation(DateTime startTime, DateTime endTime)
    {
        Update(startTime, endTime);
    }

    public void Update(DateTime startTime, DateTime endTime)
    {
        if (endTime <= startTime)
            throw new ArgumentException("End time must be after start time", nameof(endTime));

        StartTime = startTime;
        EndTime = endTime;
    }

    internal void SetParkingSpot(ParkingSpot spot)
    {
        ParkingSpot = spot;
        ParkingSpotId = spot.Id;
    }
}
