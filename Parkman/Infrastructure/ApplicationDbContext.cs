using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Parkman.Domain.Entities;

namespace Parkman.Infrastructure;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersonProfile> PersonProfiles => Set<PersonProfile>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(user =>
        {
            user.HasOne(u => u.PersonProfile)
                .WithOne(p => p.User)
                .HasForeignKey<PersonProfile>(p => p.UserId);

            user.HasOne(u => u.CompanyProfile)
                .WithOne(c => c.User)
                .HasForeignKey<CompanyProfile>(c => c.UserId);
        });

        builder.Entity<PersonProfile>(profile =>
        {
            profile.HasKey(p => p.UserId);
            profile.Property(p => p.FirstName).IsRequired();
            profile.Property(p => p.LastName).IsRequired();
            profile.Property(p => p.DateOfBirth);
            profile.Property(p => p.PhoneNumber).IsRequired();
            profile.Property(p => p.Address).IsRequired();
            profile.HasOne(p => p.CompanyProfile)
                .WithMany(c => c.Members)
                .HasForeignKey(p => p.CompanyProfileUserId);
        });

        builder.Entity<CompanyProfile>(profile =>
        {
            profile.HasKey(p => p.UserId);
            profile.Property(p => p.CompanyName).IsRequired();
            profile.Property(p => p.Ico).IsRequired();
            profile.Property(p => p.Dic).IsRequired();
            profile.Property(p => p.ContactPersonName).IsRequired();
            profile.Property(p => p.ContactEmail).IsRequired();
            profile.Property(p => p.PhoneNumber).IsRequired();
            profile.Property(p => p.BillingAddress).IsRequired();
            profile.HasMany(p => p.Vehicles)
                .WithOne(v => v.CompanyProfile)
                .HasForeignKey(v => v.CompanyProfileUserId);
            profile.HasMany(p => p.Members)
                .WithOne(m => m.CompanyProfile)
                .HasForeignKey(m => m.CompanyProfileUserId);
        });

        builder.Entity<Vehicle>(vehicle =>
        {
            vehicle.HasKey(v => v.Id);
            vehicle.Property(v => v.LicensePlate).IsRequired();
            vehicle.Property(v => v.Brand).IsRequired();
            vehicle.Property(v => v.Type).IsRequired();
            vehicle.Property(v => v.PropulsionType).IsRequired();
            vehicle.Property(v => v.IsShareable);

            vehicle.HasOne(v => v.PersonProfile)
                .WithOne(p => p.Vehicle)
                .HasForeignKey<Vehicle>(v => v.PersonProfileUserId);
        });
    }
}
