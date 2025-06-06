﻿// <auto-generated />
using System;
using GolestanProject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GolestanProject.Migrations
{
    [DbContext(typeof(MyAppContext))]
    partial class MyAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("GolestanProject.Models.classrooms", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("building")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("capacity")
                        .HasColumnType("INTEGER");

                    b.Property<int>("room_number")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("Classrooms");
                });

            modelBuilder.Entity("GolestanProject.Models.courses", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("code")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("final_exam_date")
                        .HasColumnType("TEXT");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("unit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("GolestanProject.Models.instructors", b =>
                {
                    b.Property<int>("instructor_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("hire_date")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("salary")
                        .HasColumnType("TEXT");

                    b.Property<int>("user_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("instructor_id");

                    b.HasIndex("user_id");

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("GolestanProject.Models.roles", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("GolestanProject.Models.sections", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("classroom_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("course_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("semester")
                        .HasColumnType("INTEGER");

                    b.Property<int>("time_slot_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("year")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.HasIndex("classroom_id");

                    b.HasIndex("course_id")
                        .IsUnique();

                    b.HasIndex("time_slot_id");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("GolestanProject.Models.students", b =>
                {
                    b.Property<int>("student_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("enrollment_date")
                        .HasColumnType("TEXT");

                    b.Property<int>("user_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("student_id");

                    b.HasIndex("user_id");

                    b.ToTable("Students");
                });

            modelBuilder.Entity("GolestanProject.Models.takes", b =>
                {
                    b.Property<int>("id_worthless")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float>("grade")
                        .HasColumnType("REAL");

                    b.Property<int>("section_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("student_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("id_worthless");

                    b.HasIndex("section_id");

                    b.HasIndex("student_id");

                    b.ToTable("Takes");
                });

            modelBuilder.Entity("GolestanProject.Models.teaches", b =>
                {
                    b.Property<int>("id_worthless")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("instructor_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("section_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("id_worthless");

                    b.HasIndex("instructor_id");

                    b.HasIndex("section_id");

                    b.ToTable("Teaches");
                });

            modelBuilder.Entity("GolestanProject.Models.time_slots", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("day")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("end_time")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("start_time")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Time_Slots");
                });

            modelBuilder.Entity("GolestanProject.Models.user_roles", b =>
                {
                    b.Property<int>("id_worthless")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("role_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("user_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("id_worthless");

                    b.HasIndex("role_id");

                    b.HasIndex("user_id");

                    b.ToTable("User_Roles");
                });

            modelBuilder.Entity("GolestanProject.Models.users", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("first_name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("hashed_password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("last_name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GolestanProject.Models.instructors", b =>
                {
                    b.HasOne("GolestanProject.Models.users", "USER")
                        .WithMany("INSTRUCTORS")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("USER");
                });

            modelBuilder.Entity("GolestanProject.Models.sections", b =>
                {
                    b.HasOne("GolestanProject.Models.classrooms", "CLASSROOMS")
                        .WithMany("SECTIONS")
                        .HasForeignKey("classroom_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GolestanProject.Models.courses", "COURSE")
                        .WithOne("SECTION")
                        .HasForeignKey("GolestanProject.Models.sections", "course_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GolestanProject.Models.time_slots", "TIME_SLOT")
                        .WithMany("SECTIONS")
                        .HasForeignKey("time_slot_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CLASSROOMS");

                    b.Navigation("COURSE");

                    b.Navigation("TIME_SLOT");
                });

            modelBuilder.Entity("GolestanProject.Models.students", b =>
                {
                    b.HasOne("GolestanProject.Models.users", "USER")
                        .WithMany("STUDENTS")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("USER");
                });

            modelBuilder.Entity("GolestanProject.Models.takes", b =>
                {
                    b.HasOne("GolestanProject.Models.sections", "SECTION")
                        .WithMany()
                        .HasForeignKey("section_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GolestanProject.Models.students", "STUDENT")
                        .WithMany("TAKES")
                        .HasForeignKey("student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SECTION");

                    b.Navigation("STUDENT");
                });

            modelBuilder.Entity("GolestanProject.Models.teaches", b =>
                {
                    b.HasOne("GolestanProject.Models.instructors", "INSTRUCTOR")
                        .WithMany("TEACHES")
                        .HasForeignKey("instructor_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GolestanProject.Models.sections", "SECTION")
                        .WithMany()
                        .HasForeignKey("section_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("INSTRUCTOR");

                    b.Navigation("SECTION");
                });

            modelBuilder.Entity("GolestanProject.Models.user_roles", b =>
                {
                    b.HasOne("GolestanProject.Models.roles", "ROLE")
                        .WithMany("USERROLES")
                        .HasForeignKey("role_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GolestanProject.Models.users", "USER")
                        .WithMany("USERROLES")
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ROLE");

                    b.Navigation("USER");
                });

            modelBuilder.Entity("GolestanProject.Models.classrooms", b =>
                {
                    b.Navigation("SECTIONS");
                });

            modelBuilder.Entity("GolestanProject.Models.courses", b =>
                {
                    b.Navigation("SECTION")
                        .IsRequired();
                });

            modelBuilder.Entity("GolestanProject.Models.instructors", b =>
                {
                    b.Navigation("TEACHES");
                });

            modelBuilder.Entity("GolestanProject.Models.roles", b =>
                {
                    b.Navigation("USERROLES");
                });

            modelBuilder.Entity("GolestanProject.Models.students", b =>
                {
                    b.Navigation("TAKES");
                });

            modelBuilder.Entity("GolestanProject.Models.time_slots", b =>
                {
                    b.Navigation("SECTIONS");
                });

            modelBuilder.Entity("GolestanProject.Models.users", b =>
                {
                    b.Navigation("INSTRUCTORS");

                    b.Navigation("STUDENTS");

                    b.Navigation("USERROLES");
                });
#pragma warning restore 612, 618
        }
    }
}
