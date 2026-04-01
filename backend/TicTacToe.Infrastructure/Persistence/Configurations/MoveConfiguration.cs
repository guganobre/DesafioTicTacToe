namespace TicTacToe.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

public class MoveConfiguration : IEntityTypeConfiguration<Move>
{
    public void Configure(EntityTypeBuilder<Move> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Player)
            .IsRequired();

        builder.Property(m => m.Position)
            .IsRequired();

        builder.Property(m => m.MoveOrder)
            .IsRequired();

        builder.Property(m => m.PlayedAt)
            .IsRequired();

        builder.ToTable("moves");
    }
}