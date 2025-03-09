using SkiaSharp;

namespace ImageResizer.Worker.Application.Services;

public class ImageProcessingService: IImageResizeService
{
    public string ResizeImage(string imageBase64, int width, int height)
    {
        // Create image bitmap
        var imageBytes = Convert.FromBase64String(imageBase64);
        using var inputStream = new SKMemoryStream(imageBytes);
        using var originalBitmap = SKBitmap.Decode(inputStream);

        if (originalBitmap == null)
            throw new Exception("Invalid image data for resizing!");

        // Resize
        var imageInfo = new SKImageInfo(width, height);
        using var resizedBitmap = originalBitmap.Resize(imageInfo, SKSamplingOptions.Default);
        
        if (resizedBitmap == null)
            throw new Exception("Image resizing failed!");

        // Generate resized image
        using var image = SKImage.FromBitmap(resizedBitmap);
        using var outputStream = new MemoryStream();
        image
            .Encode(SKEncodedImageFormat.Jpeg, 90)
            .SaveTo(outputStream);
        
        return Convert.ToBase64String(outputStream.ToArray());
    }
}