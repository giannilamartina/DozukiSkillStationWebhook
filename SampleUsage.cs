using System;
using System.Text.Json;

namespace DozukiSkillStationWebhook
{
    /// <summary>
    /// Sample program to demonstrate JSON deserialization of Dozuki webhook payload
    /// </summary>
    public class Program
    {
        public static void Main(string[] args)
        {
            // Sample JSON payload from Dozuki webhook
            string sampleJson = @"{
                ""event"": ""course_stage_completed"",
                ""timestamp"": 1700000000,
                ""data"": {
                    ""userId"": 12345,
                    ""userName"": ""John Doe"",
                    ""userEmail"": ""john.doe@example.com"",
                    ""courseId"": 101,
                    ""courseName"": ""Safety Training"",
                    ""stageId"": 5,
                    ""stageName"": ""Final Assessment"",
                    ""completedAt"": 1700000000
                }
            }";

            try
            {
                // Deserialize the JSON payload
                var payload = JsonSerializer.Deserialize<DozukiWebhookPayload>(sampleJson);

                // Display the parsed data
                Console.WriteLine("=== Dozuki Webhook Payload ===");
                Console.WriteLine($"Event: {payload.Event}");
                Console.WriteLine($"Timestamp: {payload.GetTimestamp()}");
                Console.WriteLine();
                Console.WriteLine("=== Event Data ===");
                Console.WriteLine($"User: {payload.Data.UserName} ({payload.Data.UserEmail})");
                Console.WriteLine($"Course: {payload.Data.CourseName} (ID: {payload.Data.CourseId})");
                Console.WriteLine($"Stage: {payload.Data.StageName} (ID: {payload.Data.StageId})");
                Console.WriteLine($"Completed At: {payload.Data.GetCompletedAt()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
