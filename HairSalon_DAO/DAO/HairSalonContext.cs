﻿
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HairSalon_BusinessObject.Models;

public partial class HairSalonContext : DbContext
{
    public HairSalonContext()
    {
    }

    public HairSalonContext(DbContextOptions<HairSalonContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableSlot> AvailableSlot { get; set; }

    public virtual DbSet<Booking> Booking { get; set; }

    public virtual DbSet<BookingDetail> BookingDetail { get; set; }

    public virtual DbSet<Earning> Earning { get; set; }

    public virtual DbSet<Payment> Payment { get; set; }

    public virtual DbSet<Role> Role { get; set; }

    public virtual DbSet<Service> Service { get; set; }

    public virtual DbSet<Slot> Slot { get; set; }

    public virtual DbSet<StylistProfile> StylistProfile { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=HairSalonService;User ID=SA;Password=12345;Encrypt=False");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableSlot>(entity =>
        {
            entity.HasKey(e => e.AvailableSlotId).HasName("PK_ServiceSlot");

            entity.Property(e => e.AvailableSlotId)
                .ValueGeneratedNever()
                .HasColumnName("availableSlotId");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.SlotId).HasColumnName("slotId");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Slot).WithMany(p => p.AvailableSlot)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceSlot_Slot");

            entity.HasOne(d => d.User).WithMany(p => p.AvailableSlot)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AvailableSlot_User");
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BookingDate)
                .HasColumnType("datetime")
                .HasColumnName("bookingDate");
            entity.Property(e => e.CreateBy).HasColumnName("createBy");
            entity.Property(e => e.Discount)
                .HasColumnType("decimal(3, 0)")
                .HasColumnName("discount");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Booking)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Booking_User1");
        });

        modelBuilder.Entity<BookingDetail>(entity =>
        {
            entity.Property(e => e.BookingDetailId).HasColumnName("bookingDetailId");
            entity.Property(e => e.AvailableSlotId).HasColumnName("availableSlotId");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ScheduledWorkingDay)
                .HasColumnType("datetime")
                .HasColumnName("scheduledWorkingDay");
            entity.Property(e => e.ServiceId).HasColumnName("serviceId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");

            entity.HasOne(d => d.AvailableSlot).WithMany(p => p.BookingDetail)
                .HasForeignKey(d => d.AvailableSlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetail_AvailableSlot");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingDetail)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetail_Booking");

            entity.HasOne(d => d.Service).WithMany(p => p.BookingDetail)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BookingDetail_Service");
        });

        modelBuilder.Entity<Earning>(entity =>
        {
            entity.Property(e => e.EarningId).HasColumnName("earningId");
            entity.Property(e => e.Commission)
                .HasColumnType("money")
                .HasColumnName("commission");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Earning)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Earning_User");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.Property(e => e.PaymentId)
                .ValueGeneratedNever()
                .HasColumnName("paymentId");
            entity.Property(e => e.Amount)
                .HasColumnType("money")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TransactionDate)
                .HasColumnType("datetime")
                .HasColumnName("transactionDate");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .HasColumnName("transactionType");

            entity.HasOne(d => d.Booking).WithMany(p => p.Payment)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payment_Booking");

        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasColumnName("roleName");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.Property(e => e.ServiceId).HasColumnName("serviceId");
            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
            entity.Property(e => e.Image)
                .HasColumnType("image")
                .HasColumnName("image");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(50)
                .HasColumnName("serviceName");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.Property(e => e.SlotId).HasColumnName("slotId");
            entity.Property(e => e.EndStart).HasColumnName("endStart");
            entity.Property(e => e.StartTime).HasColumnName("startTime");
        });

        modelBuilder.Entity<StylistProfile>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.DailySalary)
                .HasColumnType("money")
                .HasColumnName("dailySalary");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Salary)
                .HasColumnType("money")
                .HasColumnName("salary");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany()
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StylistProfile_User");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.CreatedAt)
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .HasColumnName("phoneNumber");
            entity.Property(e => e.RoleId).HasColumnName("roleId");
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .HasColumnName("userName");

            entity.HasOne(d => d.Role).WithMany(p => p.User)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}