using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DozukiSkillStationWebhook.Controllers
{
    [ApiController]
    [Route("api/dozuki")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// DEBUG: Accepts any payload and logs the raw body
        /// Use this endpoint temporarily to see what Dozuki is sending
        /// </summary>
        [HttpPost("debug")]
        public async Task<IActionResult> DebugWebhook()
        {
            Request.EnableBuffering();
            using var reader = new StreamReader(Request.Body, Encoding.UTF8, leaveOpen: true);
            var rawBody = await reader.ReadToEndAsync();
            Request.Body.Position = 0;

            _logger.LogWarning("DEBUG: Received raw body: {RawBody}", rawBody);
            _logger.LogWarning("DEBUG: Content-Type: {ContentType}", Request.ContentType);
            _logger.LogWarning("DEBUG: Headers: {Headers}", string.Join(", ", Request.Headers.Select(h => $"{h.Key}={h.Value}")));

            return Ok(new { message = "Debug: Body logged", contentType = Request.ContentType, bodyLength = rawBody.Length });
        }


        /// <summary>
        /// Receives webhook POST requests from Dozuki
        /// </summary>
        /// <param name="payload">The webhook payload</param>
        /// <returns>200 OK if successful</returns>
        [HttpPost]
        public async Task<IActionResult> ReceiveDozukiWebhook([FromBody] DozukiWebhookPayload payload)
        {
            try
            {
                // Log model state errors if any
                if (!ModelState.IsValid)
                {
                    var errors = string.Join(", ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    _logger.LogWarning("Model validation failed: {Errors}", errors);
                }

                if (payload == null)
                {
                    _logger.LogWarning("Received null payload - check request Content-Type and body format");
                    return BadRequest(new { error = "Payload is required", hint = "Ensure Content-Type is application/json" });
                }

                _logger.LogInformation(
                    "Received Dozuki webhook - Title: {Title}, Site: {SiteName}",
                    payload.Title,
                    payload.SiteName
                );

                // Process the webhook based on event title
                if (payload.Title == "Course Stage Completed")
                {
                    await ProcessCourseStageCompleted(payload.Data);
                }
                else
                {
                    _logger.LogWarning("Unknown event title: {Title}", payload.Title);
                }

                return Ok(new { message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Dozuki webhook");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Processes a Course Stage Completed event
        /// </summary>
        private async Task ProcessCourseStageCompleted(DozukiEventData data)
        {
            _logger.LogInformation(
                "Course Stage Completed - User ID: {UserId}, Stage: {StageName}, Doc ID: {DocId}, Completed: {CompletedAt}",
                data.UserId,
                data.Title,
                data.DocId,
                data.DocCompletion?.GetCompletedAt()
            );

            // TODO: Add your business logic here
            // Examples:
            // - Update database with completion status
            // - Send notification to user
            // - Trigger next stage in workflow
            // - Update external systems

            await Task.CompletedTask;
        }
    }
}
