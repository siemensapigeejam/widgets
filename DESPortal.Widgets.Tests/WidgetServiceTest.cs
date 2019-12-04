using DESPortal.Widgets.Core.Models;
using DESPortal.Widgets.Core.Services;
using DESPortal.Widgets.Infrastructure.DataAccess.DESPortal;
using DESPortal.Widgets.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DESPortal.Widgets.Tests
{
    public class WidgetServiceTest
    {
        public WidgetServiceTest()
        {

        }

        private Widget CreateWidget(string title, int dashboardId)
        {
            return new Widget()
            {
                Title = title,
                DashboardId = dashboardId,
            };
        }

        private Dashboard CreateDashboard(string name, string menuItemType, string userId)
        {
            return new Dashboard()
            {
                Name = name,
                MenuItemType = menuItemType,
                UserId = userId,
            };
        }


        [Fact]
        public void GetAll_EmptyDatabase_ReturnsEmptyList()
        {
            const string DummyUserId = "SomeUserID";
            const string DymmyUserRole = "SomeRole";
            const int DashboardId = 1;

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetAll_EmptyDatabase_ReturnsEmptyList")
               .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new DESPortalDBContext(options))
            {
                IWidgetService widgetService = new WidgetService(context);
                Assert.Empty(widgetService.GetAll(DashboardId, DummyUserId, DymmyUserRole));
            }
        }

        [Fact]
        public void GetAll_MultipleWidgets_ReturnsAllEntries()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";
            const string WidgetTitle1 = "WidgetTitle1";
            const string WidgetTitle2 = "WidgetTitle2";
            const int TotalWidgetCount = 2;

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetAll_MultipleWidgets_ReturnsAllEntries")
               .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();


                context.Widgets.Add(CreateWidget(WidgetTitle1, dashboard.Id));
                context.Widgets.Add(CreateWidget(WidgetTitle2, dashboard.Id));
                context.SaveChanges();

                IWidgetService widgetService = new WidgetService(context);
                var widgets = widgetService.GetAll(dashboard.Id, DummyUserId);
                Assert.Contains(widgets, x => x.Title == WidgetTitle1);
                Assert.Contains(widgets, x => x.Title == WidgetTitle2);
                Assert.Equal(TotalWidgetCount, widgets.Count);
            }

        }

        [Fact]
        public void GetById_NoWidgetOwner_ReturnsNull()
        {
            const string ResourceOwner = "ResourceOwner";
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";

            const string WidgetTitle1 = "WidgetTitle1";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetById_NoWidgetOwner_ReturnsNull")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, ResourceOwner);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                var createdWidget = CreateWidget(WidgetTitle1, dashboard.Id);
                context.Widgets.Add(createdWidget);
                context.SaveChanges();

                IWidgetService widgetService = new WidgetService(context);

                var widget = widgetService.GetById(createdWidget.Id, DummyUserId);
                Assert.Null(widget);
            }
        }

        [Fact]
        public void GetById_IdExistsAndUserIsValid_ReturnsWidget()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";

            const string WidgetTitle = "WidgetTitle";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetById_IdExistsAndUserIsValid_ReturnsWidget")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                var createdWidget = CreateWidget(WidgetTitle, dashboard.Id);
                context.Widgets.Add(createdWidget);
                context.SaveChanges();

                IWidgetService widgetService = new WidgetService(context);

                var widget = widgetService.GetById(createdWidget.Id, DummyUserId);
                Assert.Equal(createdWidget, widget);
            }
        }

        [Fact]
        public void Add_ValidWidget_WidgetAdded()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";

            const string WidgetTitle = "WidgetTitle";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Add_ValidWidget_WidgetAdded")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                IWidgetService widgetService = new WidgetService(context);

                var createdWidget = CreateWidget(WidgetTitle, dashboard.Id);

                widgetService.Add(createdWidget, DummyUserId);

                var widget = context.Widgets
                    .Include(x => x.Dashboard)
                    .FirstOrDefault(x => x.Id == createdWidget.Id && x.Dashboard.UserId == DummyUserId);

                Assert.NotNull(widget);
                Assert.Equal(createdWidget, widget);
            }
        }

        [Fact]
        public void Update_UpdateExistingWidget_WidgetUpdated()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";

            const string WidgetTitle = "WidgetTitle";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Update_UpdateExistingWidget_WidgetUpdated")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();
                
                var createdWidget = CreateWidget(WidgetTitle, dashboard.Id);
                context.Widgets.Add(createdWidget);
                context.SaveChanges();

                var widget = context.Widgets
                    .FirstOrDefault(x => x.Id == createdWidget.Id);

                Assert.NotNull(widget);
                Assert.Equal(createdWidget, widget);

                IWidgetService widgetService = new WidgetService(context);
                var newWidget = new Widget()
                {
                    Id = widget.Id,
                    Title = "TitleModified",
                    Height = 99,
                    Width = 99,
                    Order = 99,
                    ContentServiceId = "contentServiceIdModified",
                };
                widgetService.Update(newWidget, DummyUserId);

                widget = context.Widgets
                    .FirstOrDefault(x => x.Id == createdWidget.Id);

                Assert.NotNull(widget);
                Assert.Equal(newWidget.Id, widget.Id);
                Assert.Equal(newWidget.Title, widget.Title);
                Assert.Equal(newWidget.Height, widget.Height);
                Assert.Equal(newWidget.Width, widget.Width);
                Assert.Equal(newWidget.Order, widget.Order);
                Assert.Equal(newWidget.ContentServiceId, widget.ContentServiceId);

            }
        }

        [Fact]
        public void Delete_DeleteExistingWidget_WidgetDeleted()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenuItemType = "MenuItemType";

            const string WidgetTitle = "WidgetTitle";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Delete_DeleteExistingWidget_WidgetDeleted")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();
                
                var createdWidget = CreateWidget(WidgetTitle, dashboard.Id);
                context.Widgets.Add(createdWidget);
                context.SaveChanges();

                var widget = context.Widgets
                    .Include(x => x.Dashboard)
                    .FirstOrDefault(x => x.Id == createdWidget.Id && x.Dashboard.UserId == DummyUserId);

                Assert.NotNull(widget);
                Assert.Equal(createdWidget, widget);

                IWidgetService widgetService = new WidgetService(context);

                widgetService.Delete(widget.Id, DummyUserId);
                widget = context.Widgets
                    .Include(x => x.Dashboard)
                    .FirstOrDefault(x => x.Id == createdWidget.Id && x.Dashboard.UserId == DummyUserId);
                Assert.Null(widget);
            }
        }

    }
}
