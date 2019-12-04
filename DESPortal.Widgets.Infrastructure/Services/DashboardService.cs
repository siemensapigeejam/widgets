using DESPortal.Widgets.Core.Models;
using DESPortal.Widgets.Core.Services;
using DESPortal.Widgets.Infrastructure.DataAccess.DESPortal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DESPortal.Widgets.Infrastructure.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly DESPortalDBContext _db;

        public DashboardService(DESPortalDBContext db)
        {
            _db = db;
        }


        public List<Dashboard> GetAllDefaultDashboards()
        {
            return _db.Dashboards
                //.Where(x => !string.IsNullOrEmpty(x.MenuItemType) && !string.IsNullOrEmpty(x.UserRole))
                .Where(x => x.IsDefaultDashboard)
                .ToList();
        }

        public Dashboard GetDefaultDashboard(string menuItemType, string userRole)
        {
            userRole = userRole.ToLower();
            return _db.Dashboards
                .Include(x => x.Widgets)
                .FirstOrDefault(x => x.IsDefaultDashboard
                && x.MenuItemType == menuItemType 
                && x.UserRole.ToLower() == userRole);
        }


        public List<Dashboard> GetAll(string menuItemType, string userId)
        {
            return _db.Dashboards
                .Where(x => x.UserId == userId 
                    && x.MenuItemType == menuItemType
                    && !x.IsDefaultDashboard)
                .ToList();
        }

        public Dashboard GetById(int id, string userId)
        {
            var item = _db.Dashboards.FirstOrDefault(x => x.UserId == userId && x.Id == id);
            return item;
        }

        public Dashboard Add(Dashboard dashboard)
        {
            var result = _db.Dashboards.Add(dashboard);
            _db.SaveChanges();
            return result.Entity;
        }

        public void Update(Dashboard dashboard, string userId)
        {
            var item = _db.Dashboards
                .FirstOrDefault(x => x.Id == dashboard.Id && x.UserId == userId);
            if (item != null)
            {
                item.Name = dashboard.Name;
                _db.Dashboards.Update(item);
                _db.SaveChanges();
            }
        }

        public void Delete(int id, string userId)
        {
            var item = _db.Dashboards.FirstOrDefault(x => x.Id == id && x.UserId == userId);
            if (item != null)
            {
                _db.Dashboards.Remove(item);
                _db.SaveChanges();
            }
        }


    }
}
