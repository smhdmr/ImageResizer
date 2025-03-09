using System.Reflection;
using ImageResizer.Client.Application.Commands;
using log4net;
using log4net.Config;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));

// Add Services
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssemblyContaining<ResizeImageCommand>()
);
builder.Services.AddMassTransit(x =>
{
    var settings = builder.Configuration.GetSection("RabbitMQ");
    x.UsingRabbitMq((context, cfg) =>
    {
        var hostName = string.Join('/', settings["Host"], settings["VirtualHost"]);
        cfg.Host(hostName, host =>
        {
            host.Username(settings["Username"]!);
            host.Password(settings["Password"]!);
        });
    });
});


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

app.Run();