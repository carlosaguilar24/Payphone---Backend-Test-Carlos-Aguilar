using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransferService.Domain.Entities;

namespace TransferService.Infraestructure.Persistence.Configurations
{
    public class MovementConfiguration : IEntityTypeConfiguration<Movement>
    {
        public void Configure(EntityTypeBuilder<Movement> builder)
        {
            builder.ToTable("Movements");

            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
            .ValueGeneratedOnAdd();

            builder.Property(m => m.WalletId)
           .IsRequired();

            builder.Property(m => m.Amount)
            .HasColumnType("decimal(19,4)");

            builder.Property(m => m.Type)
           .HasConversion<string>()
           .HasMaxLength(10)
           .IsRequired();

            builder.Property(m => m.CreatedAt)
            .IsRequired();

            builder.Property(m => m.TransferId)
                .IsRequired();

            builder.HasIndex(m => m.WalletId);

            builder.HasIndex(m => new { m.TransferId, m.Type })
            .IsUnique();

            builder.HasOne<Wallet>()
                .WithMany()
                .HasForeignKey(m => m.WalletId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}