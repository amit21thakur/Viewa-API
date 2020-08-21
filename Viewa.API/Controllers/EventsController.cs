using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Viewa.Models;
using Viewa.Services;

namespace Viewa.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ILogger<EventsController> _logger;
        private readonly IEventService _eventService;

        public EventsController(ILogger<EventsController> logger, IEventService eventService)
        {
            _logger = logger;
            _eventService = eventService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetEvents(DateTime? start, DateTime? end, string eventType, string gender, string deviceType)
        {
            try
            {
                var result = await _eventService.GetEvents(start, end, eventType, gender, deviceType);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Failed to get the events.");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
