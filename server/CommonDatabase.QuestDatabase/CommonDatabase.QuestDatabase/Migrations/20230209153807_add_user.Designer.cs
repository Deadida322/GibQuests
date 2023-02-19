﻿// <auto-generated />
using CommonDatabase.QuestDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CommonDatabase.QuestDatabase.Migrations
{
    [DbContext(typeof(QuestContext))]
    [Migration("20230209153807_add_user")]
    partial class adduser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.QuestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Img")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("quest", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.CoordinatesEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<decimal>("Latitude")
                        .HasColumnType("numeric");

                    b.Property<decimal>("Longitude")
                        .HasColumnType("numeric");

                    b.Property<int>("map_stage_id")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("map_stage_id")
                        .IsUnique();

                    b.ToTable("coordinates", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                    b.Property<int>("QuestId")
                        .HasColumnType("integer");

                    b.Property<byte>("StageType")
                        .HasColumnType("smallint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("QuestId");

                    b.ToTable("Stages");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.MapStageEntity", b =>
                {
                    b.HasBaseType("CommonDatabase.QuestDatabase.Models.Stages.StageEntity");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("map_stage", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.QrCodeStageEntity", b =>
                {
                    b.HasBaseType("CommonDatabase.QuestDatabase.Models.Stages.StageEntity");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("qrcode_stage", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.TestStageEntity", b =>
                {
                    b.HasBaseType("CommonDatabase.QuestDatabase.Models.Stages.StageEntity");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("test_stage", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.TextStageEntity", b =>
                {
                    b.HasBaseType("CommonDatabase.QuestDatabase.Models.Stages.StageEntity");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("text_stage", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.VideoStageEntity", b =>
                {
                    b.HasBaseType("CommonDatabase.QuestDatabase.Models.Stages.StageEntity");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("video_stage", (string)null);
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.CoordinatesEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.MapStageEntity", "MapStage")
                        .WithOne("Coords")
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.CoordinatesEntity", "map_stage_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MapStage");
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.QuestEntity", "Quest")
                        .WithMany("Stages")
                        .HasForeignKey("QuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Quest");
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.MapStageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.MapStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.QrCodeStageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.QrCodeStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.TestStageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.TestStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.TextStageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.TextStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.VideoStageEntity", b =>
                {
                    b.HasOne("CommonDatabase.QuestDatabase.Models.Stages.StageEntity", null)
                        .WithOne()
                        .HasForeignKey("CommonDatabase.QuestDatabase.Models.Stages.VideoStageEntity", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.QuestEntity", b =>
                {
                    b.Navigation("Stages");
                });

            modelBuilder.Entity("CommonDatabase.QuestDatabase.Models.Stages.MapStageEntity", b =>
                {
                    b.Navigation("Coords")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
