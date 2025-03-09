using System.Reflection;
using Amazon;
using ImageResizer.Shared.Application.Services;
using ImageResizer.Shared.Application.Services.Contracts;
using ImageResizer.Utilities.Extensions;
using ImageResizer.Worker;
using ImageResizer.Worker.Application.Consumers;
using ImageResizer.Worker.Application.Services;
using log4net;
using log4net.Config;
using MassTransit;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly()!);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));

// Add services
builder.Services.AddScoped<IImageResizeService, ImageProcessingService>();
builder.Services.AddScoped<IFileStorageService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILog>();
    var settings = builder.Configuration.GetSection("AmazonS3");
    return new AmazonS3Service(
        logger, 
        settings["AccessKey"]!, 
        settings["SecretKey"]!, 
        RegionEndpoint.EUCentral1,
        settings["BucketName"]!
    );
});
builder.Services.AddScoped<IEmailService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILog>();
    var settings = builder.Configuration.GetSection("EmailSending");
    return new EmailService(
        logger, 
        settings["Host"]!, 
        settings["Port"]!.ToIntOrDefault(), 
        settings["Username"]!, 
        settings["Password"]!
    );
});
builder.Services.AddMassTransit(x =>
{
    var settings = builder.Configuration.GetSection("RabbitMQ");
    x.AddConsumer<ResizeImageCommandConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var hostName = string.Join('/', settings["Host"], settings["VirtualHost"]);
        cfg.Host(hostName, host =>
        {
            host.Username(settings["Username"]!);
            host.Password(settings["Password"]!);
        });
        cfg.ReceiveEndpoint("image_resize_queue", e =>
        {
            e.ConfigureConsumer<ResizeImageCommandConsumer>(context);
        });
    });
});

var host = builder.Build();
host.Run();