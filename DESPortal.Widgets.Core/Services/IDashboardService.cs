using DESPortal.Widgets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Core.Services
{
    public interface IDashboardService
    {
        List<Dashboard> GetAllDefaultDashboards();
        Dashboard GetDefaultDashboard(string menuItemType, string userRole);


        List<Dashboard> GetAll(string menuItemType, string userId);
        Dashboard GetById(int id, string userId);
        Dashboard Add(Dashboard dashboard);
        void Update(Dashboard dashboard, string userId);
        void Delete(int id, string userId);


    }
}
