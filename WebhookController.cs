using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DozukiSkillStationWebhook.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhookController : ControllerBase
    {
        private readonly ILogger<WebhookController> _logger;

        public WebhookController(ILogger<WebhookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Receives webhook POST requests from Dozuki
        /// </summary>
        /// <param name="payload">The webhook payload</param>
        /// <returns>200 OK if successful</returns>
        [HttpPost("dozuki")]
        public async Task<IActionResult> ReceiveDozukiWebhook([FromBody] DozukiWebhookPayload payload)
        {
            try
            {
                if (payload == null)
                {
                    _logger.LogWarning("Received null payload");
                    return BadRequest("Payload is required");
                }

                _logger.LogInformation(
                    "Received Dozuki webhook - Event: {Event}, Timestamp: {Timestamp}",
                    payload.Event,
                    payload.GetTimestamp()
                );

                // Process the webhook based on event type
                if (payload.Event == "course_stage_completed")
                {
                    await ProcessCourseStageCompleted(payload.Data);
                }
                else
                {
                    _logger.LogWarning("Unknown event type: {Event}", payload.Event);
                }

                return Ok(new { message = "Webhook received successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Dozuki webhook");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }

        /// <summary>
        /// Processes a Course Stage Completed event
        /// </summary>
        private async Task ProcessCourseStageCompleted(DozukiEventData data)
        {
            _logger.LogInformation(
                "Course Stage Completed - User: {UserName} ({UserEmail}), Course: {CourseName}, Stage: {StageName}, Completed: {CompletedAt}",
                data.UserName,
                data.UserEmail,
                data.CourseName,
                data.StageName,
                data.GetCompletedAt()
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
