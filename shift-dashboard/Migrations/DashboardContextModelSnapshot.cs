﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using shift_dashboard.Data;

namespace shift_dashboard.Migrations
{
    [DbContext(typeof(DashboardContext))]
    partial class DashboardContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseSerialColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("shift_dashboard.Models.Delegate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseSerialColumn();

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<double>("Approval")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Missedblocks")
                        .HasColumnType("integer");

                    b.Property<int>("PoolShare")
                        .HasColumnType("integer");

                    b.Property<int>("Producedblocks")
                        .HasColumnType("integer");

                    b.Property<double>("Productivity")
                        .HasColumnType("double precision");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("character varying(64)");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("Rate")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Vote")
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "Address" }, "Index_Address")
                        .IsUnique();

                    b.ToTable("Delegates");
                });

            modelBuilder.Entity("shift_dashboard.Models.DelegateStat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseSerialColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("DelegateId")
                        .HasColumnType("integer");

                    b.Property<int>("Rank")
                        .HasColumnType("integer");

                    b.Property<int>("TotalVoters")
                        .HasColumnType("integer");

                    b.Property<long>("TotalVotes")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DelegateId");

                    b.HasIndex(new[] { "Date" }, "Index_Date");

                    b.ToTable("DelegateStats");
                });

            modelBuilder.Entity("shift_dashboard.Models.DelegateStat", b =>
                {
                    b.HasOne("shift_dashboard.Models.Delegate", "Delegate")
                        .WithMany("DelegateStats")
                        .HasForeignKey("DelegateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Delegate");
                });

            modelBuilder.Entity("shift_dashboard.Models.Delegate", b =>
                {
                    b.Navigation("DelegateStats");
                });
#pragma warning restore 612, 618
        }
    }
}
