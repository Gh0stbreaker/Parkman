using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Parkman.Domain.Entities;
using Parkman.Infrastructure;
using Parkman.Infrastructure.Repositories.Entities;
using Parkman.Shared.Enums;
using Xunit;

namespace Parkman.Tests.Infrastructure;

public class ParkingSpotRepositoryTests
{
    [Fact]
    public async Task ListAvailableAsync_returns_only_free_spots()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        int busySpotId;
        int freeSpotId;

        using (var context = new ApplicationDbContext(options))
        {
            var lot = new ParkingLot("Lot1", "Addr1");
            var busy = new ParkingSpot("1", ParkingSpotType.Regular, ParkingSpotAccessibility.None, ParkingSpotAllowedPropulsionType.Any);
            var free = new ParkingSpot("2", ParkingSpotType.Regular, ParkingSpotAccessibility.None, ParkingSpotAllowedPropulsionType.Any);
            lot.AddSpot(busy);
            lot.AddSpot(free);
            context.ParkingLots.Add(lot);
            var reservation = new Reservation(DateTime.Parse("2024-01-01T10:00:00"), DateTime.Parse("2024-01-01T11:00:00"));
            busy.AddReservation(reservation);
            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();
            busySpotId = busy.Id;
            freeSpotId = free.Id;
        }

        using (var context = new ApplicationDbContext(options))
        {
            var repo = new ParkingSpotRepository(context, NullLogger<GenericRepository<ParkingSpot>>.Instance);
            var result = await repo.ListAvailableAsync(DateTime.Parse("2024-01-01T10:30:00"), DateTime.Parse("2024-01-01T11:30:00"));

            Assert.Contains(result, s => s.Id == freeSpotId);
            Assert.DoesNotContain(result, s => s.Id == busySpotId);
        }
    }
}
