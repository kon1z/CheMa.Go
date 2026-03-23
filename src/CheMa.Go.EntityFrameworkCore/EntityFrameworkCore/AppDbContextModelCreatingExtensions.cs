using CheMa.Go.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace CheMa.Go.EntityFrameworkCore;

public static class AppDbContextModelCreatingExtensions
{
    public static void ConfigureAppEntities(this ModelBuilder builder)
    {
        builder.Entity<Hotel>(b =>
        {
            b.ToTable(GoConsts.DbTablePrefix + "Hotels", GoConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(20);

            b.HasMany(x => x.HotelUsers).WithMany().UsingEntity(GoConsts.DbTablePrefix + "HotelIdentityUser");

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Vehicle>(b =>
        {
            b.ToTable(GoConsts.DbTablePrefix + "Vehicles", GoConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(20);
            b.Property(x => x.LicenseNum).IsRequired().HasMaxLength(8);

            b.HasIndex(x => x.LicenseNum);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Passenger>(b =>
        {
            b.ToTable(GoConsts.DbTablePrefix + "Passengers", GoConsts.DbSchema);

            b.ConfigureByConvention();

            b.Property(x => x.Name).IsRequired().HasMaxLength(20);
            b.Property(x => x.Phone).IsRequired().HasMaxLength(15);
            b.Property(x => x.Remark).IsRequired().HasMaxLength(255);

            b.OwnsOne(x => x.Location, location =>
            {
                location.Property(x => x.Name).HasColumnName(nameof(Passenger.Location) + nameof(LocationInfo.Name)).HasMaxLength(100);
                location.Property(x => x.Address).HasColumnName(nameof(Passenger.Location) + nameof(LocationInfo.Address)).HasMaxLength(255);
                location.Property(x => x.Detail).HasColumnName(nameof(Passenger.Location) + nameof(LocationInfo.Detail)).HasMaxLength(255);
            });

            b.HasOne(x => x.Hotel).WithMany(x => x.Passengers).HasForeignKey(x => x.HotelId);

            b.ApplyObjectExtensionMappings();
        });

        builder.Entity<Order>(b =>
        {
            b.ToTable(GoConsts.DbTablePrefix + "Orders", GoConsts.DbSchema);

            b.ConfigureByConvention();

            b.OwnsOne(x => x.Location, location =>
            {
                location.Property(x => x.Name).HasColumnName(nameof(Order.Location) + nameof(LocationInfo.Name)).HasMaxLength(100);
                location.Property(x => x.Address).HasColumnName(nameof(Order.Location) + nameof(LocationInfo.Address)).HasMaxLength(255);
                location.Property(x => x.Detail).HasColumnName(nameof(Order.Location) + nameof(LocationInfo.Detail)).HasMaxLength(255);
            });

            b.HasOne(x => x.Vehicle).WithMany().HasForeignKey(x => x.VehicleId);
            b.HasOne(x => x.Driver).WithMany().HasForeignKey(x => x.DriverId);
            b.HasMany(x => x.PassengerInfos).WithOne(x => x.Order).HasForeignKey(x => x.OrderId);

            b.ApplyObjectExtensionMappings();
        });
    }
}