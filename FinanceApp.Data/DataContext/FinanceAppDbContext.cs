using System;
using System.Collections.Generic;
using FinanceApp.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.Data.DataContext;

public partial class FinanceAppDbContext : DbContext
{
    public FinanceAppDbContext()
    {
    }

    public FinanceAppDbContext(DbContextOptions<FinanceAppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountEntity> Accounts { get; set; }

    public virtual DbSet<TransferEntity> Transfers { get; set; }

    public virtual DbSet<UserEntity> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=finance_app_db;Username=postgres;Password=postgres;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountEntity>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("accounts_pkey");

            entity.ToTable("accounts");

            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("0.00")
                .HasColumnType("numeric")
                .HasColumnName("balance");
            entity.Property(e => e.CurrencyType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'рубли'::character varying")
                .HasColumnName("currency_type");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_accounts_user_id");
        });

        modelBuilder.Entity<TransferEntity>(entity =>
        {
            entity.HasKey(e => e.TransferId).HasName("transfers_pkey");

            entity.ToTable("transfers");

            entity.HasIndex(e => e.AccountId, "idx_transfers_account_id");

            entity.HasIndex(e => e.AppointmentAccountId, "idx_transfers_appointment_account_id");

            entity.Property(e => e.TransferId).HasColumnName("transfer_id");
            entity.Property(e => e.AccountId).HasColumnName("account_id");
            entity.Property(e => e.AppointmentAccountId).HasColumnName("appointment_account_id");
            entity.Property(e => e.CurrencyType)
                .HasMaxLength(20)
                .HasColumnName("currency_type");
            entity.Property(e => e.TransferDate)
                .HasColumnType("timestamptz")
                .HasColumnName("transfer_date");
            entity.Property(e => e.TransferSum)
                .HasColumnType("numeric")
                .HasColumnName("transfer_sum");

            entity.HasOne(d => d.Account).WithMany(p => p.TransferAccounts)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_transfers_account_id");

            entity.HasOne(d => d.AppointmentAccount).WithMany(p => p.TransferAppointmentAccounts)
                .HasForeignKey(d => d.AppointmentAccountId)
                .HasConstraintName("fk_transfers_appointment_account_id");
        });

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.AccountCount)
                .HasDefaultValue(0)
                .HasColumnName("account_count");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .HasColumnName("first_name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .HasColumnName("last_name");
            entity.Property(e => e.MiddleName)
                .HasMaxLength(255)
                .HasColumnName("middle_name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
