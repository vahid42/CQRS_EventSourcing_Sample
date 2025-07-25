﻿// <auto-generated />
using System;
using AccountFlow.Api.V2.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountFlow.Api.V2.Migrations
{
    [DbContext(typeof(AccountDbContext))]
    [Migration("20250504140936_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("AccountFlow.Api.V2.Entities.AccountSnapshot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Created")
                        .HasColumnType("TEXT");

                    b.Property<int>("CurrentVersion")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("RowVersion")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AccountSnapshots", (string)null);
                });

            modelBuilder.Entity("AccountFlow.Api.V2.Entities.Event", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AggregateId")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventData")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("EventType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("EventVersion")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
