﻿// <auto-generated />
using EventManagement.Resources.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace EventManagement.Resources.Data.Migrations
{
    [DbContext(typeof(ResourcesDbContext))]
    [Migration("20180321061344_Dates")]
    partial class Dates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EventManagement.Resources.Data.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(500);

                    b.Property<DateTime>("End");

                    b.Property<string>("LecturerName")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<double>("Price");

                    b.Property<int?>("ResourceId");

                    b.Property<DateTime>("Start");

                    b.HasKey("Id");

                    b.HasIndex("ResourceId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EventManagement.Resources.Data.Models.Resource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Location")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.Property<int>("PlacesCount");

                    b.Property<double>("Rent");

                    b.HasKey("Id");

                    b.ToTable("Resources");
                });

            modelBuilder.Entity("EventManagement.Resources.Data.Models.Event", b =>
                {
                    b.HasOne("EventManagement.Resources.Data.Models.Resource", "Resource")
                        .WithMany()
                        .HasForeignKey("ResourceId");
                });
#pragma warning restore 612, 618
        }
    }
}
