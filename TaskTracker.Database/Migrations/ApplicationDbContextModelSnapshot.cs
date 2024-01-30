﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskTracker.Database;

#nullable disable

namespace TaskTracker.Database.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskTracker.Domain.Entity.RefreshToken", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsValid")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.SpaceUser", b =>
                {
                    b.Property<long>("SpaceId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("SpaceId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("SpaceUsers");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskAssignedUser", b =>
                {
                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("TaskId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("TaskAssignedUsers");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskFileAttachment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("TaskId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskFileAttachments");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<long>("SpaceId")
                        .HasColumnType("bigint");

                    b.Property<long>("TaskStatusGroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SpaceId");

                    b.HasIndex("TaskStatusGroupId");

                    b.ToTable("TaskLists");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskStatusGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<long?>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TaskStatusGroups");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskTrackerDocument", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreationTimestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<long>("SpaceId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SpaceId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskTrackerDocumentPage", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("DocumentId")
                        .HasColumnType("bigint");

                    b.Property<long>("LastModifiedTimestamp")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DocumentId");

                    b.ToTable("DocumentPages");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserSpace", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("OwnerId")
                        .HasColumnType("bigint");

                    b.Property<long>("StatusGroupId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("StatusGroupId");

                    b.ToTable("UserSpaces");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTask", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CreationTimestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("CreatorId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<long?>("DueTimestamp")
                        .HasColumnType("bigint");

                    b.Property<long>("ListId")
                        .HasColumnType("bigint");

                    b.Property<int?>("Priority")
                        .HasColumnType("int");

                    b.Property<long>("StatusId")
                        .HasColumnType("bigint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("ListId");

                    b.HasIndex("StatusId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTaskStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<long>("GroupId")
                        .HasColumnType("bigint");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<bool>("IsDefault")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("UserTaskStatuses");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.RefreshToken", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "User")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.SpaceUser", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.UserSpace", "Space")
                        .WithMany("Users")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.User", "User")
                        .WithMany("SpaceUsers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Space");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskAssignedUser", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.UserTask", "Task")
                        .WithMany("AssignedUsers")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.User", "User")
                        .WithMany("AssingedTasks")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Task");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskFileAttachment", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.UserTask", "Task")
                        .WithMany("Attachments")
                        .HasForeignKey("TaskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Task");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskList", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "Creator")
                        .WithMany("Lists")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.UserSpace", "Space")
                        .WithMany("Lists")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.TaskStatusGroup", "TaskStatusGroup")
                        .WithMany("Lists")
                        .HasForeignKey("TaskStatusGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Space");

                    b.Navigation("TaskStatusGroup");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskStatusGroup", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "User")
                        .WithMany("StatusGroups")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskTrackerDocument", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "Creator")
                        .WithMany("Documents")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.UserSpace", "Space")
                        .WithMany("Documents")
                        .HasForeignKey("SpaceId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Space");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskTrackerDocumentPage", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.TaskTrackerDocument", "Document")
                        .WithMany("Pages")
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserSpace", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "Owner")
                        .WithMany("Spaces")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.TaskStatusGroup", "StatusGroup")
                        .WithMany("Spaces")
                        .HasForeignKey("StatusGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("StatusGroup");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTask", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.User", "Creator")
                        .WithMany("Tasks")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.TaskList", "List")
                        .WithMany("Tasks")
                        .HasForeignKey("ListId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("TaskTracker.Domain.Entity.UserTaskStatus", "Status")
                        .WithMany("Tasks")
                        .HasForeignKey("StatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("List");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTaskStatus", b =>
                {
                    b.HasOne("TaskTracker.Domain.Entity.TaskStatusGroup", "Group")
                        .WithMany("Statuses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskList", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskStatusGroup", b =>
                {
                    b.Navigation("Lists");

                    b.Navigation("Spaces");

                    b.Navigation("Statuses");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.TaskTrackerDocument", b =>
                {
                    b.Navigation("Pages");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.User", b =>
                {
                    b.Navigation("AssingedTasks");

                    b.Navigation("Documents");

                    b.Navigation("Lists");

                    b.Navigation("RefreshTokens");

                    b.Navigation("SpaceUsers");

                    b.Navigation("Spaces");

                    b.Navigation("StatusGroups");

                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserSpace", b =>
                {
                    b.Navigation("Documents");

                    b.Navigation("Lists");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTask", b =>
                {
                    b.Navigation("AssignedUsers");

                    b.Navigation("Attachments");
                });

            modelBuilder.Entity("TaskTracker.Domain.Entity.UserTaskStatus", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}
