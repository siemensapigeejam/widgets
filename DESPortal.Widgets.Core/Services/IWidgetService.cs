using DESPortal.Widgets.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DESPortal.Widgets.Core.Services
{
    public interface IWidgetService
    {
        List<Widget> GetAll(int dashboardId, string userId, string userRole = "");

        Widget GetById(int id, string userId, string userRole = "");

        bool HasUserPermissionOnWidget(int id, string userId);

        List<Widget> GetByIds(List<int> ids, string userId);

        Widget Add(Widget widget, string userId);

        void Update(Widget widget, string userId);

        void Delete(int id, string userId);

    }
}
