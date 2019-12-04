using DESPortal.Widgets.Core.Models;
using DESPortal.Widgets.Core.Services;
using DESPortal.Widgets.Infrastructure.DataAccess.DESPortal;
using DESPortal.Widgets.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DESPortal.Widgets.Tests
{
    public class DashboardServiceTest
    {
        public DashboardServiceTest()
        {

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
            const string MenuItemType = "MenuItemType";
            const string DummyUserId = "DummyUserId";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetAll_EmptyDatabase_ReturnsEmptyList")
               .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new DESPortalDBContext(options))
            {
                IDashboardService dashboardService = new DashboardService(context);
                Assert.Empty(dashboardService.GetAll(MenuItemType, DummyUserId));
            }
        }

        [Fact]
        public void GetAll_MultipleDashboards_ReturnsAllEntries()
        {
            const string MenuItemType = "MenuItemType";
            const string DummyUserId = "DummyUserId";

            const string DashboardName1 = "DashboardName1";
            const string DashboardName2 = "DashboardName2";

            const int TotalDashboardsCount = 2;

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetAll_MultipleDashboards_ReturnsAllEntries")
               .Options;

            // Insert seed data into the database using one instance of the context
            using (var context = new DESPortalDBContext(options))
            {
                var dashboard1 = CreateDashboard(DashboardName1, MenuItemType, DummyUserId);
                var dashboard2 = CreateDashboard(DashboardName2, MenuItemType, DummyUserId);
                context.Dashboards.Add(dashboard1);
                context.Dashboards.Add(dashboard2);
                context.SaveChanges();

                IDashboardService dashboardService = new DashboardService(context);
                var dashboards = dashboardService.GetAll(MenuItemType, DummyUserId);
                Assert.Contains(dashboards, x => x.Name == DashboardName1);
                Assert.Contains(dashboards, x => x.Name == DashboardName2);
                Assert.Equal(TotalDashboardsCount, dashboards.Count);
            }

        }

        [Fact]
        public void GetById_NoDashboardOwner_ReturnsNull()
        {
            const string ResourceOwner = "ResourceOwner";
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenyItemType = "MenuItemType";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetById_NoDashboardOwner_ReturnsNull")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenyItemType, ResourceOwner);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                IDashboardService dashboardService = new DashboardService(context);

                dashboard = dashboardService.GetById(dashboard.Id, DummyUserId);
                Assert.Null(dashboard);
            }
        }

        [Fact]
        public void GetById_IdExistsAndUserIsValid_ReturnsDashboard()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenyItemType = "MenuItemType";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "GetById_IdExistsAndUserIsValid_ReturnsDashboard")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenyItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                IDashboardService dashboardService = new DashboardService(context);

                var result = dashboardService.GetById(dashboard.Id, DummyUserId);
                Assert.NotNull(result);
                Assert.Equal(dashboard, result);
            }
        }

        [Fact]
        public void Add_ValidDashboard_DashboardAdded()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenyItemType = "MenuItemType";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Add_ValidDashboard_DashboardAdded")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                IDashboardService dashboardService = new DashboardService(context);

                var dashboard = CreateDashboard(DashboardName, MenyItemType, DummyUserId);
                dashboard = dashboardService.Add(dashboard);

                var result = context.Dashboards.Find(dashboard.Id);
                Assert.NotNull(result);
                Assert.Equal(dashboard, result);
            }
        }

        [Fact]
        public void Update_UpdateExistingDashboard_DashboardUpdated()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenyItemType = "MenuItemType";

            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Update_UpdateExistingDashboard_DashboardUpdated")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenyItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();

                dashboard = context.Dashboards.Find(dashboard.Id);

                Assert.NotNull(dashboard);

                IDashboardService dashboardService = new DashboardService(context);

                var newDashboard = new Dashboard()
                {
                    Id = dashboard.Id,
                    Name = "NameMODIFIED",
                };
                dashboardService.Update(newDashboard, DummyUserId);

                newDashboard = context.Dashboards.Find(dashboard.Id);

                Assert.NotNull(dashboard);
                Assert.Equal(dashboard.Id, newDashboard.Id);
                Assert.Equal(dashboard.Name, newDashboard.Name);

            }
        }

        [Fact]
        public void Delete_DeleteExistingDashboard_DashboardDeleted()
        {
            const string DummyUserId = "SomeUserID";

            const string DashboardName = "DashboardName";
            const string MenyItemType = "MenuItemType";
            
            var options = new DbContextOptionsBuilder<DESPortalDBContext>()
               .UseInMemoryDatabase(databaseName: "Delete_DeleteExistingDashboard_DashboardDeleted")
               .Options;

            using (var context = new DESPortalDBContext(options))
            {
                var dashboard = CreateDashboard(DashboardName, MenyItemType, DummyUserId);
                context.Dashboards.Add(dashboard);
                context.SaveChanges();


                var createdDashboard = context.Dashboards
                    .FirstOrDefault(x => x.Id == dashboard.Id && x.UserId == DummyUserId);

                Assert.NotNull(createdDashboard);
                Assert.Equal(dashboard, createdDashboard);

                IDashboardService dashboardService = new DashboardService(context);

                dashboardService.Delete(createdDashboard.Id, DummyUserId);
                createdDashboard = context.Dashboards
                    .FirstOrDefault(x => x.Id == dashboard.Id && x.UserId == DummyUserId);
                Assert.Null(createdDashboard);

            }
        }

    }
}
