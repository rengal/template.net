using Microsoft.AspNetCore.Mvc;

namespace RenderWebApi.Controllers
{
    [ApiController]
    [Route("api/v0")]
    public class RenderController : ControllerBase
    {
        public class RenderRequest
        {
            public RenderRequest(string template, string data)
            {
                Template = template;
                Data = data;
            }

            /// <summary>
            /// Razor template
            /// </summary>
            public string Template { get; set; }
            public string Data { get; set; }
        }

        private readonly ILogger<RenderController> _logger;

        public RenderController(ILogger<RenderController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Renders a Razor template
        /// </summary>
        /// <param name="request">Template and data</param>
        /// <returns>The rendered XML document</returns>
        [HttpPost("render")]
        public IActionResult RenderRazorTemplate([FromBody] RenderRequest request)
        {
            var template = request.Template;
            var data = request.Data;
            return Ok("hello world");
        }
    }
}
