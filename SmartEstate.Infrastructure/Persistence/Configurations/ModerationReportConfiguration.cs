using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SmartEstate.Domain.Entities;

namespace SmartEstate.Infrastructure.Persistence.Configurations;

public class ModerationReportConfiguration : IEntityTypeConfiguration<ModerationReport>
{
    public void Configure(EntityTypeBuilder<ModerationReport> b)
    {
        b.ToTable("moderation_reports");
        b.HasKey(x => x.Id);

        b.Property(x => x.Score).IsRequired();
        b.Property(x => x.FlagsJson).HasColumnType("nvarchar(max)");
        b.Property(x => x.SuggestionsJson).HasColumnType("nvarchar(max)");
        b.Property(x => x.Decision).HasMaxLength(50).IsRequired();

        b.HasOne(x => x.Listing)
            .WithMany(x => x.ModerationReports)
            .HasForeignKey(x => x.ListingId)
            .OnDelete(DeleteBehavior.Cascade);

        b.HasIndex(x => x.ListingId);
        b.HasIndex(x => x.Decision);
        b.HasIndex(x => x.ReviewedByAdminId);
        b.HasIndex(x => x.CreatedAt);
        b.HasIndex(x => x.IsDeleted);
    }
}

