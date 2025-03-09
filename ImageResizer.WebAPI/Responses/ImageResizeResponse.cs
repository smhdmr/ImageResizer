namespace ImageResizer.WebAPI.Responses;

public record ImageResizeResponse()
{
    public Guid TaskId { get; set; }
    public string? Message { get; set; }
};