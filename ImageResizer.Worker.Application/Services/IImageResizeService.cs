namespace ImageResizer.Worker.Application.Services;

public interface IImageResizeService
{
    string ResizeImage(string imageBase64, int width, int height);
}