﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

#nullable disable

namespace StuffyHelper.EntityFrameworkCore.Migrations
{
    [DbContext(typeof(StuffyHelperContext))]
    [Migration("20220925171924_AddMedia")]
    partial class AddMedia
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PurchaseEntryPurchaseTagEntry", b =>
                {
                    b.Property<Guid>("PurchaseTagsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PurchasesId")
                        .HasColumnType("uuid");

                    b.HasKey("PurchaseTagsId", "PurchasesId");

                    b.HasIndex("PurchasesId");

                    b.ToTable("PurchaseEntryPurchaseTagEntry");
                });

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

                    b.Property<DateTime>("EventDateEnd")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("EventDateStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

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

                    b.HasIndex("Name", "EventDateStart")
                        .IsUnique();

                    b.ToTable("event", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Media.MediaEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<int>("FileType")
                        .HasColumnType("integer");

                    b.Property<string>("MediaUid")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("MediaUid")
                        .IsUnique();

                    b.ToTable("media", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Participant.ParticipantEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

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

                    b.Property<double>("Cost")
                        .HasColumnType("double precision");

                    b.Property<int>("Count")
                        .HasColumnType("integer");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("ShoppingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UnitTypeId")
                        .HasColumnType("uuid");

                    b.Property<double>("Weight")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("ShoppingId");

                    b.HasIndex("UnitTypeId");

                    b.ToTable("purchase", (string)null);
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.PurchaseTag.PurchaseTagEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("purchase-tags", (string)null);
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

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EventId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

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

            modelBuilder.Entity("StuffyHelper.Core.Features.UnitType.UnitTypeEntry", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("unit-types", (string)null);
                });

            modelBuilder.Entity("PurchaseEntryPurchaseTagEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.PurchaseTag.PurchaseTagEntry", null)
                        .WithMany()
                        .HasForeignKey("PurchaseTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StuffyHelper.Core.Features.Purchase.PurchaseEntry", null)
                        .WithMany()
                        .HasForeignKey("PurchasesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("StuffyHelper.Core.Features.Media.MediaEntry", b =>
                {
                    b.HasOne("StuffyHelper.Core.Features.Event.EventEntry", "Event")
                        .WithMany("Medias")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
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

                    b.HasOne("StuffyHelper.Core.Features.UnitType.UnitTypeEntry", "UnitType")
                        .WithMany("Purchases")
                        .HasForeignKey("UnitTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shopping");

                    b.Navigation("UnitType");
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
                    b.Navigation("Medias");

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

            modelBuilder.Entity("StuffyHelper.Core.Features.UnitType.UnitTypeEntry", b =>
                {
                    b.Navigation("Purchases");
                });
#pragma warning restore 612, 618
        }
    }
}
