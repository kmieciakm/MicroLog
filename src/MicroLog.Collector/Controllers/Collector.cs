using MicroLog.Core.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroLog.Core
{
    [ApiController]
    [Route("api/[controller]")]
    public class Collector : ControllerBase
    {
        private ILogPublisher _LogPublisher { get; set; }

        public Collector(ILogPublisher logPublisher)
        {
            _LogPublisher = logPublisher;
        }

        [AllowAnonymous]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert(LogEvent logEvent)
        {
            try
            {
                await _LogPublisher.PublishAsync(logEvent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
