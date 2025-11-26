using System;
using System.Text.Json.Serialization;

namespace DozukiSkillStationWebhook
{
    /// <summary>
    /// Represents the complete webhook payload from Dozuki
    /// </summary>
    public class DozukiWebhookPayload
    {
        [JsonPropertyName("event")]
        public string Event { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("data")]
        public DozukiEventData Data { get; set; }

        /// <summary>
        /// Gets the timestamp as a DateTime object
        /// </summary>
        public DateTime GetTimestamp()
        {
            return DateTimeOffset.FromUnixTimeSeconds(Timestamp).DateTime;
        }
    }

    /// <summary>
    /// Represents the event data for a Course Stage Completed event
    /// </summary>
    public class DozukiEventData
    {
        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("userName")]
        public string UserName { get; set; }

        [JsonPropertyName("userEmail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("courseId")]
        public int CourseId { get; set; }

        [JsonPropertyName("courseName")]
        public string CourseName { get; set; }

        [JsonPropertyName("stageId")]
        public int StageId { get; set; }

        [JsonPropertyName("stageName")]
        public string StageName { get; set; }

        [JsonPropertyName("completedAt")]
        public long CompletedAt { get; set; }

        /// <summary>
        /// Gets the completion timestamp as a DateTime object
        /// </summary>
        public DateTime GetCompletedAt()
        {
            return DateTimeOffset.FromUnixTimeSeconds(CompletedAt).DateTime;
        }
    }
}
