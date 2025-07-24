using System;
using System.Collections.Generic;

namespace Parkman.Shared.Entities;

public class Reservation
{
    public int Id { get; private set; }

    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }

    public int ParkingSpotId { get; private set; }
    public ParkingSpot ParkingSpot { get; private set; } = null!;

    private readonly List<ProfileReservation> _profileReservations = new();
    public IReadOnlyCollection<ProfileReservation> ProfileReservations => _profileReservations;
    private readonly List<CompanyReservation> _companyReservations = new();
    public IReadOnlyCollection<CompanyReservation> CompanyReservations => _companyReservations;

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

    internal void AddProfileReservation(ProfileReservation link)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));
        _profileReservations.Add(link);
    }

    internal void AddCompanyReservation(CompanyReservation link)
    {
        if (link == null) throw new ArgumentNullException(nameof(link));
        _companyReservations.Add(link);
    }
}
