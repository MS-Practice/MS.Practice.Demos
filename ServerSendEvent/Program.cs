using Lib.AspNetCore.ServerSentEvents;
using Microsoft.AspNetCore.ResponseCompression;
using ServerSendEvent.HostServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddServerSentEvents();
builder.Services.AddResponseCompression(options => {
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "text/event-stream" });
});
//builder.Services.AddHostedService<HeartbeatService>();
builder.Services.AddHostedService<ServerEventsWorker>();

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

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCompression()
    .UseStaticFiles()
    .UseRouting()
    .UseEndpoints(endpoint => {
        endpoint.MapControllers();
        //endpoint.MapServerSentEvents("/see-heartbeat");
        endpoint.MapServerSentEvents("/chat-completion");
    })
    ;

app.Run();
