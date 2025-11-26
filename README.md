# Dozuki Webhook Server - Quick Start

## Running the Server

To start the webhook server, run:

```powershell
dotnet run
```

The server will start on:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

## Webhook Endpoint

Your Dozuki webhook endpoint is:
```
POST http://localhost:5000/api/webhook/dozuki
```

## Using with ngrok

Since you have ngrok running on port 5000, your public webhook URL is:
```
https://your-ngrok-url.ngrok.io/api/webhook/dozuki
```

Configure this URL in Dozuki's webhook settings.

## Testing the Webhook

### Using curl:
```powershell
curl -X POST http://localhost:5000/api/webhook/dozuki `
  -H "Content-Type: application/json" `
  -d '{
    "event": "course_stage_completed",
    "timestamp": 1700000000,
    "data": {
      "userId": 12345,
      "userName": "John Doe",
      "userEmail": "john.doe@example.com",
      "courseId": 101,
      "courseName": "Safety Training",
      "stageId": 5,
      "stageName": "Final Assessment",
      "completedAt": 1700000000
    }
  }'
```

### Using the .http file:
Open `DozukiSkillStationWebhook.http` in Visual Studio Code and click "Send Request"

## Viewing Logs

The webhook logs will appear in your console when events are received. Look for messages like:
```
Received Dozuki webhook - Event: course_stage_completed, Timestamp: ...
Course Stage Completed - User: John Doe (john.doe@example.com), Course: Safety Training, Stage: Final Assessment
```

## Project Structure

```
DozukiSkillStationWebhook/
├── Controllers/
│   └── WebhookController.cs    # Webhook endpoint handler
├── DozukiModels.cs              # Data models for webhook payloads
├── Program.cs                   # Application entry point
└── appsettings.json             # Configuration
```

## Next Steps

1. **Run the server**: `dotnet run`
2. **Get your ngrok URL**: Check the ngrok terminal for your public URL
3. **Configure Dozuki**: Add `https://your-ngrok-url.ngrok.io/api/webhook/dozuki` to Dozuki's webhook settings
4. **Test**: Trigger a "Course Stage Completed" event in Dozuki
5. **Customize**: Add your business logic in `ProcessCourseStageCompleted()` method
