using MediatR;

namespace ImageResizer.Client.Application.Commands;

public record ResizeImageCommand: IRequest<Guid>
{
    public required string ImageBase64 { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public required string Email { get; set; }
}