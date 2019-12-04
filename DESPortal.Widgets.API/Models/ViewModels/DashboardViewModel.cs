using DESPortal.Widgets.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DESPortal.Widgets.API.Models.ViewModels
{
    public class DashboardInfoViewModel
    {
        public DashboardInfoViewModel()
        {

        }

        private DashboardInfoViewModel(Dashboard dashboard)
        {
            Id = dashboard.Id;
            Name = dashboard.Name;
        }

        public List<DashboardInfoViewModel> Transform(List<Dashboard> dashboards)
        {
            var result = new List<DashboardInfoViewModel>();
            foreach(var dashboard in dashboards)
            {
                result.Add(new DashboardInfoViewModel(dashboard));
            }
            return result;
        }

        public int Id { get; set; }

        public string  Name { get; set; }

    }

    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            WidgetIds = new List<int>();
        }

        public DashboardViewModel(Dashboard dashboard)
        {
            Id = dashboard.Id;
            Name = dashboard.Name;
            WidgetIds = dashboard.Widgets?.Select(x => x.Id).ToList();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public List<int> WidgetIds { get; set; }

    }

    public class DashboardPostViewModel
    {

        public Dashboard Tranform(DashboardPostViewModel dashboard)
        {
            Dashboard result = new Dashboard()
            {
                Name = dashboard.Name,
                MenuItemType = dashboard.MenuItemType,
                UserId = dashboard.UserId,
            };
            return result;
        }

        public string Name { get; set; }

        public string MenuItemType { get; set; }

        public string UserId { get; set; }
        
    }

    public class DashboardPutViewModel
    {

        public Dashboard Tranform(int id, DashboardPutViewModel dashboard)
        {
            Dashboard result = new Dashboard()
            {
                Id = id,
                Name = dashboard.Name,
            };
            return result;
        }

        public string Name { get; set; }
        
        public string UserId { get; set; }

    }

}
