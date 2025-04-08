﻿// <auto-generated />
using System;
using DataServices.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataServices.Migrations
{
    [DbContext(typeof(BhxhDbContext))]
    [Migration("20250406013157_removeuser")]
    partial class Removeuser
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataServices.Entities.Human.Department", b =>
                {
                    b.Property<int>("DepartmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DepartmentId"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActivity")
                        .HasColumnType("bit");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SortOrder")
                        .HasColumnType("int");

                    b.HasKey("DepartmentId");

                    b.HasIndex("LevelId");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("EmployeeId"));

                    b.Property<string>("AccountBank")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Birthdate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("DeptId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Gender")
                        .HasColumnType("bit");

                    b.Property<string>("IdentityCard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsQuitJob")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.Property<int?>("SalaryCoefficientId")
                        .HasColumnType("int");

                    b.Property<int?>("SortOrder")
                        .HasColumnType("int");

                    b.Property<long?>("TelegramId")
                        .HasColumnType("bigint");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DeptId");

                    b.HasIndex("PostId");

                    b.HasIndex("SalaryCoefficientId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Level", b =>
                {
                    b.Property<int>("LevelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LevelId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LevelId");

                    b.ToTable("Level");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Position", b =>
                {
                    b.Property<int>("PositionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PositionId"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PositionId");

                    b.ToTable("Positions");
                });

            modelBuilder.Entity("DataServices.Entities.Human.QuarterDepartmentRank", b =>
                {
                    b.Property<int>("QuarterDepartmentRankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuarterDepartmentRankId"));

                    b.Property<int>("DeptId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Quarter")
                        .HasColumnType("tinyint");

                    b.Property<int?>("ResultScore")
                        .HasColumnType("int");

                    b.Property<int>("RewardId")
                        .HasColumnType("int");

                    b.Property<int?>("SelfScore")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("QuarterDepartmentRankId");

                    b.HasIndex("DeptId");

                    b.HasIndex("RewardId");

                    b.ToTable("QuarterDepartmentRanks");
                });

            modelBuilder.Entity("DataServices.Entities.Human.QuarterEmployeeRank", b =>
                {
                    b.Property<int>("QuarterEmployeeRankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("QuarterEmployeeRankId"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("NumWorked")
                        .HasColumnType("int");

                    b.Property<byte>("Quarter")
                        .HasColumnType("tinyint");

                    b.Property<int?>("ResultScore")
                        .HasColumnType("int");

                    b.Property<int>("RewardId")
                        .HasColumnType("int");

                    b.Property<int?>("SelfScore")
                        .HasColumnType("int");

                    b.Property<int?>("TotalWork")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("QuarterEmployeeRankId");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RewardId");

                    b.ToTable("QuarterEmployeeRanks");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Reward", b =>
                {
                    b.Property<int>("RewardId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RewardId"));

                    b.Property<string>("Classify")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShortName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RewardId");

                    b.ToTable("Rewards");
                });

            modelBuilder.Entity("DataServices.Entities.Human.SalaryCoefficient", b =>
                {
                    b.Property<int>("SalaryCoefficientId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SalaryCoefficientId"));

                    b.Property<double>("Coeficient")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("Rank")
                        .HasColumnType("tinyint");

                    b.HasKey("SalaryCoefficientId");

                    b.ToTable("SalaryCoefficients");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Department", b =>
                {
                    b.HasOne("DataServices.Entities.Human.Level", "Level")
                        .WithMany("Departments")
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Level");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Employee", b =>
                {
                    b.HasOne("DataServices.Entities.Human.Department", "Dept")
                        .WithMany("Employees")
                        .HasForeignKey("DeptId");

                    b.HasOne("DataServices.Entities.Human.Position", "Post")
                        .WithMany("Employees")
                        .HasForeignKey("PostId");

                    b.HasOne("DataServices.Entities.Human.SalaryCoefficient", "SalaryCoefficient")
                        .WithMany("Employees")
                        .HasForeignKey("SalaryCoefficientId");

                    b.Navigation("Dept");

                    b.Navigation("Post");

                    b.Navigation("SalaryCoefficient");
                });

            modelBuilder.Entity("DataServices.Entities.Human.QuarterDepartmentRank", b =>
                {
                    b.HasOne("DataServices.Entities.Human.Department", "Dept")
                        .WithMany("QuarterDepartmentRanks")
                        .HasForeignKey("DeptId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataServices.Entities.Human.Reward", "Reward")
                        .WithMany("QuarterDepartmentRanks")
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dept");

                    b.Navigation("Reward");
                });

            modelBuilder.Entity("DataServices.Entities.Human.QuarterEmployeeRank", b =>
                {
                    b.HasOne("DataServices.Entities.Human.Employee", "Employee")
                        .WithMany("QuarterEmployeeRanks")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataServices.Entities.Human.Reward", "Reward")
                        .WithMany("QuarterEmployeeRanks")
                        .HasForeignKey("RewardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Reward");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Department", b =>
                {
                    b.Navigation("Employees");

                    b.Navigation("QuarterDepartmentRanks");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Employee", b =>
                {
                    b.Navigation("QuarterEmployeeRanks");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Level", b =>
                {
                    b.Navigation("Departments");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Position", b =>
                {
                    b.Navigation("Employees");
                });

            modelBuilder.Entity("DataServices.Entities.Human.Reward", b =>
                {
                    b.Navigation("QuarterDepartmentRanks");

                    b.Navigation("QuarterEmployeeRanks");
                });

            modelBuilder.Entity("DataServices.Entities.Human.SalaryCoefficient", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
