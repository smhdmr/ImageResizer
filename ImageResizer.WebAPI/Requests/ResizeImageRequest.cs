namespace ImageResizer.WebAPI.Requests;

public record ResizeImageRequest()
{
    public required string ImageBase64 { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public required string Email { get; set; }
};