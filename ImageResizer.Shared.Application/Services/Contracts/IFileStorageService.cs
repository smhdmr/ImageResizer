namespace ImageResizer.Shared.Application.Services.Contracts;

public interface IFileStorageService
{
    Task<string> UploadFile(string fileBase64, string fileName);
}