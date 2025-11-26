var builder = WebApplication.CreateBuilder(args);

// Configure to listen on port 5000 (HTTP only for ngrok)
builder.WebHost.UseUrls("http://localhost:5000");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Removed HTTPS redirection for ngrok compatibility
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

