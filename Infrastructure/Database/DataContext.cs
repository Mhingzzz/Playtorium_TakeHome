using System;
using System.Collections.Generic;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database;

public partial class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AppliedDiscounts> AppliedDiscounts { get; set; }

    public virtual DbSet<Cart> Cart { get; set; }

    public virtual DbSet<CartItems> CartItems { get; set; }

    public virtual DbSet<Customers> Customers { get; set; }

    public virtual DbSet<DiscountCampaigns> DiscountCampaigns { get; set; }

    public virtual DbSet<Items> Items { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppliedDiscounts>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("applied_discounts_pkey");

            entity.ToTable("applied_discounts");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AppliedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("applied_at");
            entity.Property(e => e.CampaignId).HasColumnName("campaign_id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.DiscountAmount)
                .HasPrecision(10, 2)
                .HasColumnName("discount_amount");
            entity.Property(e => e.DiscountType)
                .HasMaxLength(255)
                .HasColumnName("discount_type");

            entity.HasOne(d => d.Campaign).WithMany(p => p.AppliedDiscounts)
                .HasForeignKey(d => d.CampaignId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("applied_discounts_campaign_id_fkey");

            entity.HasOne(d => d.Cart).WithMany(p => p.AppliedDiscounts)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("applied_discounts_cart_id_fkey");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_pkey");

            entity.ToTable("cart");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CustomerId).HasColumnName("customer_id");
            entity.Property(e => e.Status)
                .HasMaxLength(255)
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("total_price");

            entity.HasOne(d => d.Customer).WithMany(p => p.Cart)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_customer_id_fkey");
        });

        modelBuilder.Entity<CartItems>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("cart_items_pkey");

            entity.ToTable("cart_items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CartId).HasColumnName("cart_id");
            entity.Property(e => e.ItemId).HasColumnName("item_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(1)
                .HasColumnName("quantity");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_items_cart_id_fkey");

            entity.HasOne(d => d.Item).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cart_items_item_id_fkey");
        });

        modelBuilder.Entity<Customers>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("customers_pkey");

            entity.ToTable("customers");

            entity.HasIndex(e => e.Email, "customers_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Points)
                .HasDefaultValue(0)
                .HasColumnName("points");
        });

        modelBuilder.Entity<DiscountCampaigns>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("discount_campaigns_pkey");

            entity.ToTable("discount_campaigns");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CampaignType)
                .HasMaxLength(255)
                .HasColumnName("campaign_type");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.DiscountValue)
                .HasPrecision(10, 2)
                .HasColumnName("discount_value");
            entity.Property(e => e.DiscountYThb)
                .HasPrecision(10, 2)
                .HasColumnName("discount_y_thb");
            entity.Property(e => e.EveryXThb)
                .HasPrecision(10, 2)
                .HasColumnName("every_x_thb");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.ItemCategory)
                .HasMaxLength(255)
                .HasColumnName("item_category");
            entity.Property(e => e.PointsCap)
                .HasPrecision(10, 2)
                .HasColumnName("points_cap");
        });

        modelBuilder.Entity<Items>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("items_pkey");

            entity.ToTable("items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .HasColumnName("category");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("name");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("0")
                .HasColumnName("price");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
