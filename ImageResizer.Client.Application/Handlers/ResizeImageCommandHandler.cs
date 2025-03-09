using ImageResizer.Client.Application.Commands;
using ImageResizer.Messaging.Messages;
using log4net;
using MassTransit;
using MediatR;

namespace ImageResizer.Client.Application.Handlers;

public class ResizeImageCommandHandler(
        ILog logger, 
        IPublishEndpoint publishEndpoint
    ): IRequestHandler<ResizeImageCommand, Guid>
{
    public async Task<Guid> Handle(ResizeImageCommand request, CancellationToken cancellationToken)
    {
        var taskId = Guid.NewGuid();
        var message = new ResizeImageMessage
        {
            TaskId = taskId,
            ImageBase64 = request.ImageBase64,
            Width = request.Width,
            Height = request.Height,
            Email = request.Email
        };
        
        await publishEndpoint.Publish(message, cancellationToken);
        logger.Info($"Image resize task published: {taskId}");

        return taskId;
    }
}