using Hangfire;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddHangfire(x => x.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddHangfireServer();

// Add the processing server as IHostedService
builder.Services.AddHangfireServer();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHangfireDashboard("/dashboard");

IBackgroundJobClient backgroundJobs = new BackgroundJobClient();
backgroundJobs.Enqueue(()=>Console.WriteLine("Hello from hangfire at Program.cs"));
backgroundJobs.Enqueue(()=>Console.WriteLine("Hello from hangfire at Program.cs"));

app.Run();
