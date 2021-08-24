﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShippingCostCalculator.Data;

namespace ShippingCostCalculator.Data.Migrations
{
    [DbContext(typeof(ShippingContext))]
    [Migration("20210824213911_AddTimeStamp")]
    partial class AddTimeStamp
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.9");

            modelBuilder.Entity("ShippingCostCalculator.Data.Courier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Couriers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Cargo4You"
                        },
                        new
                        {
                            Id = 2,
                            Name = "ShipFaster"
                        },
                        new
                        {
                            Id = 3,
                            Name = "MaltaShip"
                        });
                });

            modelBuilder.Entity("ShippingCostCalculator.Data.ShippingData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CourierId")
                        .HasColumnType("INTEGER");

                    b.Property<float>("Height")
                        .HasColumnType("REAL");

                    b.Property<float>("Length")
                        .HasColumnType("REAL");

                    b.Property<float>("ShippingCost")
                        .HasColumnType("REAL");

                    b.Property<DateTimeOffset>("TimeStamp")
                        .HasColumnType("TEXT");

                    b.Property<float>("Weight")
                        .HasColumnType("REAL");

                    b.Property<float>("Width")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("CourierId");

                    b.ToTable("ShippingData");
                });

            modelBuilder.Entity("ShippingCostCalculator.Data.ShippingData", b =>
                {
                    b.HasOne("ShippingCostCalculator.Data.Courier", "Courier")
                        .WithMany()
                        .HasForeignKey("CourierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Courier");
                });
#pragma warning restore 612, 618
        }
    }
}
