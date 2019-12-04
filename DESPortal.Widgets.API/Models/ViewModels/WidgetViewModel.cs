using DESPortal.Widgets.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DESPortal.Widgets.API.Models.ViewModels
{
    public class WidgetViewModel
    {
        public WidgetViewModel()
        {
            WidgetDimensions = new WidgetDimensions();
        }

        public WidgetViewModel(Widget widget)
        {
            Id = widget.Id;
            Title = widget.Title;
            ContentServiceId = widget.ContentServiceId;
            WidgetDimensions = new WidgetDimensions()
            {
                Height = widget.Height,
                Width = widget.Width,
            };
            Order = widget.Order;
        }

        public List<WidgetViewModel> Transform(List<Widget> widgets)
        {
            List<WidgetViewModel> result = new List<WidgetViewModel>();
            foreach (var widget in widgets)
            {
                result.Add(new WidgetViewModel(widget));
            }
            return result;
        }

        public Widget WidgetViewModelTWidget(WidgetViewModel widget, string userId)
        {
            Widget result = new Widget()
            {
                //Id = widget.Id,
                //Title = widget.Title,
                //MenuItemType = widget.MenuItemType,
                //ContentServiceId = widget.ContentServiceId,
                //Height = widget.WidgetDimensions.Height,
                //Width = widget.WidgetDimensions.Width,
                //Order = widget.Order,
                //UserId = userId,
            };
            return result;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string ContentServiceId { get; set; }
        
        public WidgetDimensions WidgetDimensions { get; set; }

        public int Order { get; set; }
        
    }

    public class WidgetDimensions
    {
        public int Height { get; set; }

        public int Width { get; set; }
    }

    public class WidgetPutViewModel
    {

        public WidgetPutViewModel()
        {
            Dimensions = new WidgetDimensions();
        }

        public Widget WidgetPutViewModelToWidget(int id, WidgetPutViewModel widget)
        {
            Widget result = new Widget()
            {
                Id = id,
                Title = widget.Title,
                ContentServiceId = widget.ContentServiceId,
                Height = widget.Dimensions.Height,
                Width = widget.Dimensions.Width,
                Order = widget.Order,
            };
            return result;
        }

        public string UserId { get; set; }

        public string Title { get; set; }

        public string ContentServiceId { get; set; }
        
        public WidgetDimensions Dimensions { get; set; }

        public int Order { get; set; }
    }

    public class WidgetPostViewModel
    {
        public WidgetPostViewModel()
        {
            Dimensions = new WidgetDimensions();
        }

        public Widget WidgetPostViewModelToWidget(WidgetPostViewModel widget)
        {
            Widget result = new Widget()
            {
                Title = widget.Title,
                ContentServiceId = widget.ContentServiceId,
                Height = widget.Dimensions.Height,
                Width = widget.Dimensions.Width,
                Order = widget.Order,
                DashboardId = widget.DashboardId,
            };
            return result;
        }

        public string UserId { get; set; }

        public int DashboardId { get; set; }

        public string Title { get; set; }

        public string ContentServiceId { get; set; }
        
        public WidgetDimensions Dimensions { get; set; }

        public int Order { get; set; }

    }

    public class WidgetOrder
    {
        public Widget ChangeOrder(Widget widget, int order)
        {
            widget.Order = order;
            return widget;
        }

        public List<Widget> ChangeWidgetsOrders(List<Widget> widgets, List<WidgetOrder> ordering)
        {
            var result = widgets
                .Select(x => ChangeOrder(x, ordering.FirstOrDefault(xx => xx.WidgetId == x.Id).OrderId))
                .ToList();
            return result;
        }

        public int WidgetId { get; set; }

        public int OrderId { get; set; }

    }

    public class WidgetsOrderViewModel
    {
        public WidgetsOrderViewModel()
        {
            WidgetOrders = new List<WidgetOrder>();
        }

        public List<WidgetOrder> WidgetOrders { get; set; }

        public string UserId { get; set; }

    }

}
