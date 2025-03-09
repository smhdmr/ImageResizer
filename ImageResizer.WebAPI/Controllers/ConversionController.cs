using ImageResizer.Client.Application.Commands;
using ImageResizer.WebAPI.Requests;
using ImageResizer.WebAPI.Responses;
using log4net;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ImageResizer.WebAPI.Controllers;

[ApiController]
[Route("convert")]
public class ConversionController(
        ILog logger, 
        IMediator mediator
    ): ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> ResizeImage(ResizeImageRequest request)
    {
        var command = new ResizeImageCommand
        {
            ImageBase64 = request.ImageBase64,
            Width = request.Width,
            Height = request.Height,
            Email = request.Email
        };
        var taskId = await mediator.Send(command);

        var response = new ImageResizeResponse()
        {
            TaskId = taskId,
            Message = "Image resize task received."
        };
        return Accepted(response);
    }
}