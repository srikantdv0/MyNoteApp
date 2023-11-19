﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Notes.DbContexts;

#nullable disable

namespace Notes.Migrations
{
    [DbContext(typeof(NoteContext))]
    [Migration("20230516161334_DST to DTS")]
    partial class DSTtoDTS
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.16");

            modelBuilder.Entity("Notes.Entities.Note", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedDTS")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ModifiedBy")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ModifiedDTS")
                        .HasColumnType("TEXT");

                    b.Property<int>("OwnerId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Notes.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Permissions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "R"
                        },
                        new
                        {
                            Id = 2,
                            Description = "RW"
                        },
                        new
                        {
                            Id = 3,
                            Description = "RWU"
                        },
                        new
                        {
                            Id = 4,
                            Description = "RWUD"
                        });
                });

            modelBuilder.Entity("Notes.Entities.SharedNote", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("NoteId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PermissionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SharedToId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("NoteId");

                    b.HasIndex("PermissionId");

                    b.HasIndex("SharedToId");

                    b.ToTable("SharedNotes");
                });

            modelBuilder.Entity("Notes.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDTS")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedDTS = new DateTime(2023, 5, 16, 16, 13, 34, 713, DateTimeKind.Utc).AddTicks(9030),
                            Email = "srikantdv0@gmail.com",
                            IsActive = true,
                            Password = "Password123"
                        },
                        new
                        {
                            Id = 2,
                            CreatedDTS = new DateTime(2023, 5, 16, 16, 13, 34, 713, DateTimeKind.Utc).AddTicks(9030),
                            Email = "srikant.yadav@gmail.com",
                            IsActive = true,
                            Password = "Srikant@123"
                        });
                });

            modelBuilder.Entity("Notes.Entities.Note", b =>
                {
                    b.HasOne("Notes.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Notes.Entities.SharedNote", b =>
                {
                    b.HasOne("Notes.Entities.Note", "Note")
                        .WithMany("SharedNote")
                        .HasForeignKey("NoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notes.Entities.Permission", "Permission")
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Notes.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("SharedToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Note");

                    b.Navigation("Permission");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Notes.Entities.Note", b =>
                {
                    b.Navigation("SharedNote");
                });
#pragma warning restore 612, 618
        }
    }
}
