﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(StuffyHelperContext))]
    partial class StuffyHelperContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StuffyHelper.Core.Features.Event.EventEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsCompleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name", "EventDate")
                        .IsUnique();

                    b.ToTable("event", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Participant.ParticipantEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("UserId", "EventId")
                        .IsUnique();

                    b.ToTable("participant", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Purchase.PurchaseEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Amount")
                        .HasColumnType("double precision");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ShoppingId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ShoppingId");

                    b.ToTable("purchase", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.PurchaseUsage.PurchaseUsageEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PurchaseId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseId");

                    b.HasIndex("ParticipantId", "PurchaseId")
                        .IsUnique();

                    b.ToTable("purchase-usage", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Shopping.ShoppingEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Check")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ParticipantId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ShoppingDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("ParticipantId");

                    b.HasIndex("ShoppingDate");

                    b.ToTable("shopping", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Participant.ParticipantEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.Event.EventEntry", "Event")
                        .WithMany("Participants")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Purchase.PurchaseEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.Shopping.ShoppingEntry", "Shopping")
                        .WithMany("Purchases")
                        .HasForeignKey("ShoppingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shopping");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.PurchaseUsage.PurchaseUsageEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.Participant.ParticipantEntry", "Participant")
                        .WithMany("PurchaseUsages")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StuffyHelper.Core.Features.Purchase.PurchaseEntry", "Purchase")
                        .WithMany("PurchaseUsages")
                        .HasForeignKey("PurchaseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Participant");

                    b.Navigation("Purchase");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Shopping.ShoppingEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.Event.EventEntry", "Event")
                        .WithMany("Shoppings")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StuffyHelper.Core.Features.Participant.ParticipantEntry", "Participant")
                        .WithMany("Shoppings")
                        .HasForeignKey("ParticipantId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Participant");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Event.EventEntry", b =>
                {
                    b.Navigation("Participants");

                    b.Navigation("Shoppings");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Participant.ParticipantEntry", b =>
                {
                    b.Navigation("PurchaseUsages");

                    b.Navigation("Shoppings");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Purchase.PurchaseEntry", b =>
                {
                    b.Navigation("PurchaseUsages");
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Shopping.ShoppingEntry", b =>
                {
                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
