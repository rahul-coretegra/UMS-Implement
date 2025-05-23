﻿using Microsoft.EntityFrameworkCore;
using User_Management_System_Implementation.Models;
using Route = User_Management_System_Implementation.Models.Route;

namespace User_Management_System_Implementation
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<RoleAccess> RoleAccess { get; set; }

        public DbSet<Route> Routes { get; set; }

        public DbSet<RouteAccess> RouteAccess { get; set; }

        public DbSet<Menu> Menus { get; set; }

        public DbSet<MenuAccess> MenuAccess { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<ConfigureService> ConfigureServices { get; set; }

        public DbSet<UserVerification> UserVerifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<RoleAccess>()
                .HasOne(ura => ura.User)
                .WithMany()
                .HasForeignKey(ura => ura.UserId);

            modelBuilder.Entity<RoleAccess>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);

            modelBuilder.Entity<RouteAccess>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);

            modelBuilder.Entity<RouteAccess>()
                .HasOne(ura => ura.Route)
                .WithMany()
                .HasForeignKey(ura => ura.RouteId);

            modelBuilder.Entity<MenuAccess>()
                .HasOne(ura => ura.UserRole)
                .WithMany()
                .HasForeignKey(ura => ura.RoleId);

            modelBuilder.Entity<MenuAccess>()
                .HasOne(ura => ura.Menu)
                .WithMany()
                .HasForeignKey(ura => ura.MenuId);

            modelBuilder.Entity<ConfigureService>()
                .HasOne(ura => ura.Service)
                .WithMany()
                .HasForeignKey(ura => ura.ServiceId);

        }
    }
}
