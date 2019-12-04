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
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        /// <summary>
        /// Returns Dashboards
        /// </summary>
        /// <returns>All Dashboards </returns>
        /// <response code="200">Returns all dashboards</response>
        /// <response code="403">If userId is not valid </response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public ActionResult<List<DashboardInfoViewModel>> Get(string menuItemType, string userId)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(403);
            }
            return new DashboardInfoViewModel().Transform(_dashboardService.GetAll(menuItemType, userId)).ToList();
        }

        /// <summary>
        /// Returns Dashboard
        /// </summary>
        /// <returns>Dashboard </returns>
        /// <response code="200">Returns Dashboard by Id</response>
        /// <response code="403">If userId is not valid </response>
        [HttpGet("{id}", Name = "GetDashboard")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public ActionResult<DashboardViewModel> GetDashboard(int Id, string userId)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(403);
            }
            return new DashboardViewModel(_dashboardService.GetById(Id, userId));
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
        /// <response code="403">If userId is not valid </response>       
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public ActionResult<DashboardViewModel> Post([FromBody]DashboardPostViewModel model)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                return StatusCode(403);
            }
            if (!ModelState.IsValid)
            {
                return StatusCode(400);
            }
            try
            {
                var item = _dashboardService.Add(new DashboardPostViewModel().Tranform(model));
                if (item == null)
                {
                    return StatusCode(400);
                }
                return Ok();
                //return CreatedAtRoute("GetDashboard", new { Id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(403);
            }
        }


        /// <summary>
        /// Edit a Dashboard.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Dashboards/id
        ///     {
        ///         "userId": "1",
        ///         "name": "Dashboard title",
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created Dashboard</returns>
        /// <response code="200">If item modified</response>
        /// <response code="204">If item can not be edited</response>
        /// <response code="403">If userId is not valid </response>   
        /// <response code="422">If validation error</response>      
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        public IActionResult Put(int id, [FromBody]DashboardPutViewModel model)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(model.UserId))
            {
                return StatusCode(403);
            }

            if (!ModelState.IsValid)
            {
                return StatusCode(422);
            }

            var item = _dashboardService.GetById(id, model.UserId);
            if (item == null)
            {
                return NoContent();
            }

            try
            {
                _dashboardService.Update(new DashboardPutViewModel().Tranform(id, model), model.UserId);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return NoContent();
            }

            return Ok();
        }


        /// <summary>
        /// Deletes a specific Dashboard
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">If item removed</response>
        /// <response code="204">If item can not be removed</response>
        /// <response code="403">If userId is not valid </response>     
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        public IActionResult Delete(int id, string userId)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(403);
            }
            var item = _dashboardService.GetById(id, userId);
            if (item == null)
            {
                return NoContent();
            }
            _dashboardService.Delete(id, userId);
            return Ok();
        }
        
    }
}