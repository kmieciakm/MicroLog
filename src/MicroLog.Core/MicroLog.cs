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
    [Route("api/sink")]
    public class MicroLog : ControllerBase
    {
        private ILogSink _LogSink { get; set; }

        public MicroLog(ILogSink logSink)
        {
            _LogSink = logSink;
        }

        [AllowAnonymous]
        [HttpPost("insert")]
        public async Task<IActionResult> Insert(LogEvent logEvent)
        {
            try
            {
                await _LogSink.InsertAsync(logEvent);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
