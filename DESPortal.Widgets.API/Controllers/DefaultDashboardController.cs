using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DESPortal.Widgets.API.Models.ViewModels;
using DESPortal.Widgets.Core.Models;
using DESPortal.Widgets.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DESPortal.Widgets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefaultDashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DefaultDashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Returns Default Dashboards
        /// </summary>
        /// <returns>All Default Dashboards </returns>
        /// <response code="200">Returns all Default Dashboards</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public ActionResult<List<DefaultDashboardInfoViewModel>> Get()
        {
            return new DefaultDashboardInfoViewModel().Transform(_dashboardService.GetAllDefaultDashboards()).ToList();
        }

        /// <summary>
        /// Returns Default Dashboard
        /// </summary>
        /// <returns>Default Dashboard </returns>
        /// <response code="200">Returns Default Dashboard by userRole and menuItemType</response>
        /// <response code="204">Returns if such dashboard does not exist </response>
        [HttpGet("{menuItemType}/{userRole}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public ActionResult<DashboardViewModel> Get(string menuItemType, string userRole)
        {
            var result = _dashboardService.GetDefaultDashboard(menuItemType, userRole);
            if(result == null)
            {
                return NoContent();
            }
            return new DashboardViewModel(result);
        }

        /// <summary>
        /// Creates a Dashboard.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST / Dashboards
        ///     {
        ///        "userId": "1",
        ///        "name": "Item1",
        ///        "menuItemType": "group",
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created Dashboard</returns>
        /// <response code="200">Returns when item created </response>
        /// <response code="400">If the item is null</response>         
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public ActionResult<DashboardViewModel> Post([FromBody]DefaultDashboardViewModel model)
        {
            //string userId = Request.Headers["UserId"];
            //if (string.IsNullOrWhiteSpace(model.UserId))
            //{
            //    return StatusCode(403);
            //}
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            try
            {
                var item = _dashboardService.Add(new DefaultDashboardViewModel().Transform(model));
                if (item == null)
                {
                    return StatusCode(400);
                }
                return Ok();
                //return CreatedAtRoute("GetDefaultDashboard", new { menuItemType = item.MenuItemType, userRole = item.UserRole }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(400);
            }
        }


    }
}