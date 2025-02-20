﻿// <auto-generated />
using System;
using FinanceApp.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceApp.Data.Migrations
{
    [DbContext(typeof(FinanceAppDbContext))]
    [Migration("20250208110654_hype08.02")]
    partial class hype0802
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("FinanceApp.Data.DataModels.AccountEntity", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("account_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AccountId"));

                    b.Property<decimal>("Balance")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("money")
                        .HasColumnName("balance")
                        .HasDefaultValueSql("0.00");

                    b.Property<string>("CurrencyType")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("currency_type")
                        .HasDefaultValueSql("'рубли'::character varying");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("AccountId")
                        .HasName("accounts_pkey");

                    b.HasIndex("UserId");

                    b.ToTable("accounts", (string)null);
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.TransferEntity", b =>
                {
                    b.Property<int>("TransferId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("transfer_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TransferId"));

                    b.Property<int>("AccountId")
                        .HasColumnType("integer")
                        .HasColumnName("account_id");

                    b.Property<int>("AppointmentAccountId")
                        .HasColumnType("integer")
                        .HasColumnName("appointment_account_id");

                    b.Property<string>("CurrencyType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("currency_type");

                    b.Property<DateTime>("TransferDate")
                        .HasColumnType("timestamp(0) without time zone")
                        .HasColumnName("transfer_date");

                    b.Property<decimal>("TransferSum")
                        .HasColumnType("money")
                        .HasColumnName("transfer_sum");

                    b.HasKey("TransferId")
                        .HasName("transfers_pkey");

                    b.HasIndex(new[] { "AccountId" }, "idx_transfers_account_id");

                    b.HasIndex(new[] { "AppointmentAccountId" }, "idx_transfers_appointment_account_id");

                    b.ToTable("transfers", (string)null);
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.UserEntity", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<int>("AccountCount")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0)
                        .HasColumnName("account_count");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("email");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("first_name");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("middle_name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("password");

                    b.HasKey("UserId")
                        .HasName("users_pkey");

                    b.HasIndex(new[] { "Email" }, "users_email_key")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.AccountEntity", b =>
                {
                    b.HasOne("FinanceApp.Data.DataModels.UserEntity", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_accounts_user_id");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.TransferEntity", b =>
                {
                    b.HasOne("FinanceApp.Data.DataModels.AccountEntity", "Account")
                        .WithMany("TransferAccounts")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transfers_account_id");

                    b.HasOne("FinanceApp.Data.DataModels.AccountEntity", "AppointmentAccount")
                        .WithMany("TransferAppointmentAccounts")
                        .HasForeignKey("AppointmentAccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_transfers_appointment_account_id");

                    b.Navigation("Account");

                    b.Navigation("AppointmentAccount");
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.AccountEntity", b =>
                {
                    b.Navigation("TransferAccounts");

                    b.Navigation("TransferAppointmentAccounts");
                });

            modelBuilder.Entity("FinanceApp.Data.DataModels.UserEntity", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
