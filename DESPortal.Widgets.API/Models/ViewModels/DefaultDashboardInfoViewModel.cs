using DESPortal.Widgets.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DESPortal.Widgets.API.Models.ViewModels
{
    public class DefaultDashboardInfoViewModel
    {
        public DefaultDashboardInfoViewModel()
        {

        }

        public DefaultDashboardInfoViewModel(Dashboard dashboard)
        {
            Id = dashboard.Id;
            MenuItemType = dashboard.MenuItemType;
            UserRole = dashboard.UserRole;
        }

        public List<DefaultDashboardInfoViewModel> Transform(List<Dashboard> dashboards)
        {
            var result = new List<DefaultDashboardInfoViewModel>();
            foreach(var dashboard in dashboards)
            {
                result.Add(new DefaultDashboardInfoViewModel(dashboard));
            }
            return result;
        }

        public int Id { get; set; }

        public string MenuItemType { get; set; }

        public string UserRole { get; set; }

    }

    public class DefaultDashboardViewModel
    {

        public Dashboard Transform(DefaultDashboardViewModel model)
        {
            return new Dashboard()
            {
                Name = "Default",
                MenuItemType = model.MenuItemType,
                UserRole = model.UserRole,
                UserId = model.UserId,
                IsDefaultDashboard = true,
            };
        }

        public string MenuItemType { get; set; }

        public string UserRole { get; set; }

        public string UserId { get; set; }
        
    }



}
