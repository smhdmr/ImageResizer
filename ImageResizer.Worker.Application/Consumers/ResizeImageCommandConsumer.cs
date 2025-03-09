using ImageResizer.Messaging.Messages;
using ImageResizer.Shared.Application.Services.Contracts;
using ImageResizer.Worker.Application.Services;
using log4net;
using MassTransit;

namespace ImageResizer.Worker.Application.Consumers;

public class ResizeImageCommandConsumer(
        ILog logger,
        IImageResizeService imageResizeService,
        IFileStorageService fileStorageService,
        IEmailService emailService
    ): IConsumer<ResizeImageMessage>
{
    public async Task Consume(ConsumeContext<ResizeImageMessage> context)
    {
        // To simulate a long-running task
        await Task.Delay(30_000).WaitAsync(CancellationToken.None);
        
        // Resize image
        var request = context.Message;
        var resizedImageBase64 = imageResizeService.ResizeImage(request.ImageBase64, request.Width, request.Height);
        
        // Upload image to file storage
        var fileUrl = await fileStorageService.UploadFile(resizedImageBase64, $"{request.TaskId}.jpg");
        
        // Send file link via email
        _ = emailService.SendEmail(
            request.Email, 
            "ImageResizer - Your Image is Ready!", 
            $"Here is the url to download your image. Link will expire in 24 hours.\n\n{fileUrl}"
        );
        logger.Info($"Image processed: {request.ImageBase64}");
    }
}