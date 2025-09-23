using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyManager.Models;

namespace MoneyManager.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id); // Primary key

            builder.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(x => x.Type)
                .HasConversion<int>(); // Store enum as int in the database for better compatibility.

            builder.Property(x => x.Description)
                .HasMaxLength(150);

            builder.HasOne(x => x.Category)
                .WithMany()
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Category if it has related Transactions.

            builder.HasIndex(x => x.Date); // Index on Date for faster queries.
            builder.HasIndex(x => x.CategoryId); // Index on CategoryId for faster lookups.

        }
    }
}
