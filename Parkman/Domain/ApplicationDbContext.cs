using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Parkman.Domain;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<PersonProfile> PersonProfiles => Set<PersonProfile>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();

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
        });

        builder.Entity<CompanyProfile>(profile =>
        {
            profile.HasKey(p => p.UserId);
            profile.Property(p => p.CompanyName).IsRequired();
            profile.Property(p => p.Ico).IsRequired();
            profile.Property(p => p.Dic).IsRequired();
        });
    }
}
