using Microsoft.EntityFrameworkCore;
using Server.Persistence.Schemas;

namespace Server.Persistence;

internal class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    internal DbSet<UserSchema> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSchema>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Id)
                .ValueGeneratedOnAdd();
            entity
                .Property(u => u.Identifier)
                .IsRequired()
                .HasMaxLength(50);
            entity
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(50);
        });
    }
}