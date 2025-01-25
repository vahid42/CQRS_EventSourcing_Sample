﻿// <auto-generated />
using System;
using AccountApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountApi.Migrations
{
    [DbContext(typeof(AccountDbContext))]
    partial class AccountDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.12");

            modelBuilder.Entity("AccountApi.Entities.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Balance")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Accounts", (string)null);
                });

            modelBuilder.Entity("AccountApi.Entities.Event", b =>
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

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
