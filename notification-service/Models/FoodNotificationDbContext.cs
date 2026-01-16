using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace notification_service.Models;

public partial class FoodNotificationDbContext : DbContext
{
    public FoodNotificationDbContext()
    {
    }

    public FoodNotificationDbContext(DbContextOptions<FoodNotificationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notification> Notifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("notification_status", new[] { "Pending", "Sent", "Failed", "Retrying" })
            .HasPostgresEnum("notification_type", new[] { "Email", "SMS", "Push" });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notifications_pkey");

            entity.ToTable("notifications");

            entity.HasIndex(e => e.Createdat, "idx_notifications_created_at");

            entity.HasIndex(e => e.Userid, "idx_notifications_user_id");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasColumnName("id");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnName("createdat");
            entity.Property(e => e.Providerresponse).HasColumnName("providerresponse");
            entity.Property(e => e.Recipient)
                .HasMaxLength(255)
                .HasColumnName("recipient");
            entity.Property(e => e.Updatedat)
                .HasDefaultValueSql("now()")
                .HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");



            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property<string>("Type")
                      .HasColumnName("type");

                entity.Property<string>("Status")
                      .HasColumnName("status");
            });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
