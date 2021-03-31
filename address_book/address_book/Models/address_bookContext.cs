using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace address_book.Models
{
    public partial class address_bookContext : DbContext
    {
        public address_bookContext()
        {
        }

        public address_bookContext(DbContextOptions<address_bookContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Contactswithnumber> Contactswithnumbers { get; set; }
        public virtual DbSet<PhoneNumber> PhoneNumbers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Host=localhost;Database=address_book;Username=postgres;Password=admin");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Croatian_Croatia.1252");

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("contacts");

                entity.HasIndex(e => e.Address, "address")
                    .IsUnique();

                entity.HasIndex(e => e.Name, "name")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Contactswithnumber>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("contactswithnumbers");

                entity.Property(e => e.Address).HasColumnName("address");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("date_of_birth");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.Numbers).HasColumnName("numbers");
            });

            modelBuilder.Entity<PhoneNumber>(entity =>
            {
                entity.ToTable("phone_numbers");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContactsId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("contacts_id");

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnName("number");

                entity.HasOne(d => d.Contacts)
                    .WithMany(p => p.PhoneNumbers)
                    .HasForeignKey(d => d.ContactsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("contacts_id");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
