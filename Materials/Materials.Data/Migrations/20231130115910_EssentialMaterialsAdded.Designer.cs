﻿// <auto-generated />
using Materials.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Materials.Data.Migrations
{
    [DbContext(typeof(MaterialsDbContext))]
    [Migration("20231130115910_EssentialMaterialsAdded")]
    partial class EssentialMaterialsAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Materials.Data.EssentialMaterial", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("DataText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TemplateId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("EssentialMaterials");
                });

            modelBuilder.Entity("Materials.Data.MaterialData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CourseId")
                        .HasColumnType("integer");

                    b.Property<string>("DataText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Materials");
                });
#pragma warning restore 612, 618
        }
    }
}
