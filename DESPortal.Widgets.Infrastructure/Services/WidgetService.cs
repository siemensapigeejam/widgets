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
    public class WidgetService : IWidgetService
    {
        private readonly DESPortalDBContext _db;

        public WidgetService(DESPortalDBContext db)
        {
            _db = db;
        }

        public List<Widget> GetAll(int dashboardId, string userId, string userRole = "")
        {
            return _db.Widgets
                .Include(x => x.Dashboard)
                .Where(x => x.DashboardId == dashboardId 
                && (x.Dashboard.UserId == userId 
                || x.Dashboard.UserRole == userRole))
                .ToList();
        }

        public Widget GetById(int id, string userId, string userRole = "")
        {
            var item = _db.Widgets
                .Include(x => x.Dashboard)
                .FirstOrDefault(x => x.Id == id
                && (x.Dashboard.UserId == userId || x.Dashboard.UserRole == userRole));
            return item;
        }

        public bool HasUserPermissionOnWidget(int id, string userId)
        {
            var result = _db.Widgets
                .Include(x => x.Dashboard)
                .Any(x => x.Id == id
                && x.Dashboard.UserId == userId);
            return result;
        }

        public Widget Add(Widget widget, string userId)
        {
            var allowed = (_db.Dashboards.Find(widget.DashboardId).UserId == userId);
            if(allowed)
            {
                var result = _db.Widgets.Add(widget);
                _db.SaveChanges();
                return result.Entity;
            }
            else
            {
                return null;
            }
        }

        public void Update(Widget widget, string userId)
        {
            var item = _db.Widgets
                .Include(x => x.Dashboard)
                .FirstOrDefault(x => x.Id == widget.Id && x.Dashboard.UserId == userId);
            if (item != null)
            {
                item.Title = widget.Title;
                item.ContentServiceId = widget.ContentServiceId;
                item.Order = widget.Order;
                item.Width= widget.Width;
                item.Height = widget.Height;

                _db.SaveChanges();
            }
        }

        public List<Widget> GetByIds(List<int> ids, string userId)
        {
            var result = new List<Widget>();
            foreach(var id in ids)
            {
                result.Add(_db.Widgets.Include(x => x.Dashboard).FirstOrDefault(x => x.Id == id && x.Dashboard.UserId == userId));
            }
            return result;
        }

        public void Delete(int id, string userId)
        {
            var item = _db.Widgets
                .Include(x => x.Dashboard)
                .FirstOrDefault(x => x.Id == id && x.Dashboard.UserId == userId);
            if (item != null)
            {
                _db.Widgets.Remove(item);
                _db.SaveChanges();
            }
        }

    }
}
