using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Parkman.Domain.Entities;
using Parkman.Infrastructure;
using Parkman.Infrastructure.Repositories;
using Xunit;

namespace Parkman.Tests.Infrastructure;

public class PaginationTests
{
    [Fact]
    public async Task ListPagedAsync_returns_total_count_and_items()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using (var context = new ApplicationDbContext(options))
        {
            context.ParkingLots.Add(new ParkingLot("Lot1", "Addr1"));
            context.ParkingLots.Add(new ParkingLot("Lot2", "Addr2"));
            context.ParkingLots.Add(new ParkingLot("Lot3", "Addr3"));
            await context.SaveChangesAsync();
        }

        using (var context = new ApplicationDbContext(options))
        {
            var repo = new GenericRepository<ParkingLot>(context);
            var result = await repo.ListPagedAsync(skip:1, take:1);

            Assert.Equal(3, result.TotalCount);
            Assert.Single(result.Items);
        }
    }
}
