using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using ImageResizer.Shared.Application.Services.Contracts;
using log4net;

namespace ImageResizer.Shared.Application.Services;

public class AmazonS3Service(
        ILog logger, 
        string accessKey, 
        string secretKey, 
        RegionEndpoint region,
        string bucketName
    ) : IFileStorageService
{
    private readonly AmazonS3Client s3Client = new(accessKey, secretKey, region);

    public async Task<string> UploadFile(string fileBase64, string fileName)
    {
        try
        {
            // Get file stream
            var fileBytes = Convert.FromBase64String(fileBase64);
            using var stream = new MemoryStream(fileBytes);
            
            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = bucketName,
                ContentType = "image/jpeg"
            };

            // Upload the file
            var fileTransferUtility = new TransferUtility(s3Client);
            await fileTransferUtility.UploadAsync(uploadRequest);
            
            // Generate pre-signed url to allow temporary access
            var request = new GetPreSignedUrlRequest
            {
                BucketName = bucketName,
                Key = fileName,
                Expires = DateTime.UtcNow.AddDays(1)
            };
            var preSignedUrl = await s3Client.GetPreSignedURLAsync(request);
            if (preSignedUrl == null)
            {
                var errorMessage = $"Failed to get pre-signed AWS S3 file url for file: {fileName}";
                logger.Error(errorMessage);
                throw new Exception(errorMessage);
            }
            
            logger.Info($"Image uploaded to AWS S3: {preSignedUrl}");
            return preSignedUrl;
        }
        catch (Exception ex)
        {
            logger.Error("Error uploading image to AWS S3.", ex);
            throw;
        }
    }
}