﻿// <auto-generated />
using System;
using ClubStats.AspNetCore.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ClubStats.AspNetCore.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20220622015312_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Attendee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("MeetingId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SignedIn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("SignedOut")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("MemberId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Meeting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Organization", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.OrganizationMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("OrganizationId")
                        .HasColumnType("uuid");

                    b.Property<int>("PermissionLevel")
                        .HasColumnType("integer");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("OrganizationId");

                    b.HasIndex("UserId");

                    b.ToTable("OrganizationMembers");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GroupOrganizationMember", b =>
                {
                    b.Property<Guid>("GroupsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MemberHoldersId")
                        .HasColumnType("uuid");

                    b.HasKey("GroupsId", "MemberHoldersId");

                    b.HasIndex("MemberHoldersId");

                    b.ToTable("GroupOrganizationMember");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Attendee", b =>
                {
                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.Meeting", "Meeting")
                        .WithMany("Attendees")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.OrganizationMember", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Group", b =>
                {
                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.Organization", "Organization")
                        .WithMany("Groups")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Meeting", b =>
                {
                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.Organization", null)
                        .WithMany("Meetings")
                        .HasForeignKey("OrganizationId");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.OrganizationMember", b =>
                {
                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.Organization", "Organization")
                        .WithMany("Members")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.User", "User")
                        .WithMany("OrganizationMembers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GroupOrganizationMember", b =>
                {
                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubStats.AspNetCore.DataAccess.Entities.OrganizationMember", null)
                        .WithMany()
                        .HasForeignKey("MemberHoldersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Meeting", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.Organization", b =>
                {
                    b.Navigation("Groups");

                    b.Navigation("Meetings");

                    b.Navigation("Members");
                });

            modelBuilder.Entity("ClubStats.AspNetCore.DataAccess.Entities.User", b =>
                {
                    b.Navigation("OrganizationMembers");
                });
#pragma warning restore 612, 618
        }
    }
}
