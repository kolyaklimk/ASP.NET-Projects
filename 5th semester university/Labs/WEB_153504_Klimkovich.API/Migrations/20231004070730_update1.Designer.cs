﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WEB_153504_Klimkovich.API.Data;

#nullable disable

namespace WEB_153504_Klimkovich.API.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20231004070730_update1")]
    partial class update1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("WEB_153504_Klimkovich.Domain.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("WEB_153504_Klimkovich.Domain.Entities.Electronics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .HasColumnType("TEXT");

                    b.Property<string>("Mime")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Electronics");
                });

            modelBuilder.Entity("WEB_153504_Klimkovich.Domain.Entities.Electronics", b =>
                {
                    b.HasOne("WEB_153504_Klimkovich.Domain.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
