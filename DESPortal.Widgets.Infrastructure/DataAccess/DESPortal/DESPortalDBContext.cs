using DESPortal.Widgets.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Infrastructure.DataAccess.DESPortal
{
    public class DESPortalDBContext : DbContext
    {
        public DbSet<Widget> Widgets { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }


        public DESPortalDBContext(DbContextOptions<DESPortalDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Widget>()
                .HasOne(p => p.Dashboard)
                .WithMany(b => b.Widgets)
                .HasForeignKey(p => p.DashboardId);
        }
    }
}
