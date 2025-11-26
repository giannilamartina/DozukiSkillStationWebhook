using System;
using System.Text.Json.Serialization;

namespace DozukiSkillStationWebhook
{
    /// <summary>
    /// Represents the complete webhook payload from Dozuki
    /// </summary>
    public class DozukiWebhookPayload
    {
        [JsonPropertyName("siteName")]
        public string SiteName { get; set; }

        [JsonPropertyName("siteDomain")]
        public string SiteDomain { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("data")]
        public DozukiEventData Data { get; set; }
    }

    /// <summary>
    /// Represents the event data for a Course Stage Completed event
    /// </summary>
    public class DozukiEventData
    {
        [JsonPropertyName("course_stageid")]
        public int CourseStageId { get; set; }

        [JsonPropertyName("docid")]
        public int DocId { get; set; }

        [JsonPropertyName("doctype")]
        public string DocType { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("userid")]
        public int UserId { get; set; }

        [JsonPropertyName("doc_completion")]
        public DozukiDocCompletion DocCompletion { get; set; }
    }

    public class DozukiDocCompletion
    {
        [JsonPropertyName("course_assignment_stage_completionid")]
        public int CourseAssignmentStageCompletionId { get; set; }

        [JsonPropertyName("init_date")]
        public long InitDate { get; set; }

        [JsonPropertyName("end_date")]
        public long EndDate { get; set; }

        /// <summary>
        /// Gets the completion timestamp as a DateTime object
        /// </summary>
        public DateTime GetCompletedAt()
        {
            return DateTimeOffset.FromUnixTimeSeconds(EndDate).DateTime;
        }
    }

    
}
