using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using TeamSite.Models;

namespace TeamSite.Migrations
{
    [DbContext(typeof(AADbContext))]
    [Migration("20170728205110_Teams")]
    partial class Teams
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TeamSite.Models.AccountTeam", b =>
                {
                    b.Property<int>("AccountTeamID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("TeamName");

                    b.HasKey("AccountTeamID");

                    b.ToTable("AccountTeams");
                });

            modelBuilder.Entity("TeamSite.Models.Client", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountTeamID");

                    b.Property<string>("ClientName");

                    b.Property<int>("EcoSureClientID");

                    b.HasKey("ClientID");

                    b.HasIndex("AccountTeamID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("TeamSite.Models.Contact", b =>
                {
                    b.Property<int>("ContactID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.HasKey("ContactID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("TeamSite.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("AccountTeamID");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<int?>("ManagerEmployeeID");

                    b.Property<string>("Phone");

                    b.Property<string>("Position");

                    b.HasKey("EmployeeID");

                    b.HasIndex("AccountTeamID");

                    b.HasIndex("ManagerEmployeeID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("TeamSite.Models.Client", b =>
                {
                    b.HasOne("TeamSite.Models.AccountTeam")
                        .WithMany("Clients")
                        .HasForeignKey("AccountTeamID");
                });

            modelBuilder.Entity("TeamSite.Models.Employee", b =>
                {
                    b.HasOne("TeamSite.Models.AccountTeam")
                        .WithMany("Employees")
                        .HasForeignKey("AccountTeamID");

                    b.HasOne("TeamSite.Models.Employee", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerEmployeeID");
                });
        }
    }
}
