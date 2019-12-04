using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DESPortal.Widgets.API.Models.ViewModels;
using DESPortal.Widgets.Core.Models;
using DESPortal.Widgets.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace DESPortal.Widgets.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WidgetsController : ControllerBase
    {
        private readonly IWidgetService _widgetService;

        public WidgetsController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        /// <summary>
        /// Returns Widgets
        /// </summary>
        /// <returns>All Widgets </returns>
        /// <response code="200">Returns all widgets</response>
        /// <response code="403">If userId is not valid </response>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public ActionResult<List<WidgetViewModel>> Get(int dashboardId, string userId, string userRole)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(403);
            }
            return new WidgetViewModel().Transform(_widgetService.GetAll(dashboardId, userId, userRole)).OrderBy(x => x.Order).ToList();
        }

        /// <summary>
        /// Returns Widget
        /// </summary>
        /// <returns>Widget </returns>
        /// <response code="200">Returns Widget by Id</response>
        /// <response code="403">If userId is not valid </response>
        [HttpGet("{id}", Name = "GetWidget")]
        [ProducesResponseType(200)]
        [ProducesResponseType(403)]
        public ActionResult<WidgetViewModel> GetWidget(int id, string userId, string userRole)
        {
            //string userId = Request.Headers["UserId"];
            if (string.IsNullOrWhiteSpace(userId))
            {
                return StatusCode(403);
            }
            return new WidgetViewModel(_widgetService.GetById(id, userId, userRole));
        }


        /// <summary>
        /// Creates a Widget.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Widgets
        ///     {
        ///        "userId": "1",
        ///         "title": "Widget title",
        ///         "contentServiceId": "1",
        ///         "dimensions" : {
        ///	            "width": 2,
        ///	            "height": 1
        ///         },
        ///         "order": 1,
        ///         "DashboardId": 1,
        ///        
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created Widget</returns>
        /// <response code="200">Returns when item created </response>
        /// <response code="400">If the item is null</response>    
        /// <response code="403">If userId is not valid </response>       
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public ActionResult<WidgetViewModel> Post([FromBody]WidgetPostViewModel model)
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
                var item = _widgetService.Add(new WidgetPostViewModel().WidgetPostViewModelToWidget(model), model.UserId);
                if (item == null)
                {
                    return StatusCode(400);
                }
                return Ok();
                //return CreatedAtRoute("GetWidget", new { Id = item.Id }, item);
            }
            catch (Exception ex)
            {
                return StatusCode(403);
            }
        }

        /// <summary>
        /// Edit a Widget.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Widgets/id
        ///     {
        ///         "userId": "1",
        ///         "title": "Widget title",
        ///         "contentServiceId": "1",
        ///         "dimensions" : {
        ///	            "width": 2,
        ///	            "height": 1
        ///         },
        ///         "menuItemType": "Widget",
        ///         "order": 1
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created Widget</returns>
        /// <response code="200">If item modified</response>
        /// <response code="204">If item can not be edited</response>
        /// <response code="403">If userId is not valid </response>   
        /// <response code="422">If validation error</response>      
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        public IActionResult Put(int id, [FromBody]WidgetPutViewModel model)
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
            var hasPermission = _widgetService.HasUserPermissionOnWidget(id, model.UserId);
            if (!hasPermission)
            {
                return NoContent();
            }
            try
            {
                _widgetService.Update(new WidgetPutViewModel().WidgetPutViewModelToWidget(id, model), model.UserId);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return NoContent();
            }
            return Ok();
        }

        /// <summary>
        /// Deletes a specific Widget.
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
            var hasPermission = _widgetService.HasUserPermissionOnWidget(id, userId);
            if (!hasPermission)
            {
                return NoContent();
            }
            _widgetService.Delete(id, userId);
            return Ok();
        }

        /// <summary>
        /// Edit a Widget ordering
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /Widgets/
        ///     {
        ///         UserId: "1",
        ///         WidgetOrders:
        ///             [{ 
        ///                 WidgetId: 1,
        ///                 OrderId: 2,
        ///             },
        ///             {
        ///                WidgetId: 2,
        ///                 OrderId: 1,       
        ///             }]
        ///     }
        ///
        /// </remarks>
        /// <param name="model"></param>
        /// <returns>A newly created Widget</returns>
        /// <response code="200">If items modified</response>
        /// <response code="204">If items can not be edited</response>
        /// <response code="403">If userId is not valid </response>   
        /// <response code="422">If validation error</response>      
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [ProducesResponseType(403)]
        [ProducesResponseType(422)]
        public IActionResult Put([FromBody]WidgetsOrderViewModel model)
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
            var items = _widgetService.GetByIds(model.WidgetOrders.Select(x => x.WidgetId).ToList(), model.UserId);
            if (items.Any(x => x == null))
            {
                return StatusCode(403);
            }
            try
            {
                new WidgetOrder().ChangeWidgetsOrders(items, model.WidgetOrders).ForEach(x => _widgetService.Update(x, model.UserId));
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return NoContent();
            }
            return Ok();
        }

    }
}