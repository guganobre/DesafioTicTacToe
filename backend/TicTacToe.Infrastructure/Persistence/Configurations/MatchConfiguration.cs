namespace TicTacToe.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TicTacToe.Domain.Entities;

public class MatchConfiguration : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Player1Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Player2Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(m => m.Winner)
            .HasMaxLength(100);

        builder.Property(m => m.Result)
            .IsRequired();

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.HasMany(m => m.Moves)
            .WithOne()
            .HasForeignKey(mv => mv.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.ToTable("matches");
    }
}
