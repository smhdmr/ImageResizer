namespace ImageResizer.Messaging.Messages;

public record ResizeImageMessage
{
    public Guid TaskId { get; set; }
    public required string ImageBase64 { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public required string Email { get; set; }
}