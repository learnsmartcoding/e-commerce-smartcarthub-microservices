using Azure.Storage.Blobs;

namespace Products.Service
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string containerName, string folderName, string fileName);
    }

    public class StorageService : IStorageService
    {
        private readonly string _connectionString; // Add your Azure Storage connection string here

        public StorageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string containerName,
            string folderName, string fileName)
        {
            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                // Create the container if it doesn't exist
                await blobContainerClient.CreateIfNotExistsAsync();

                // Combine folder name and file name using a delimiter (e.g., '/')
                var blobName = Path.Combine(folderName, fileName).Replace("\\", "/");

                // Get a reference to a blob
                var blobClient = blobContainerClient.GetBlobClient(blobName);

                // Upload the image
                await blobClient.UploadAsync(imageStream, true);

                // Get the full path of the uploaded file
                var blobUri = blobClient.Uri;
                return blobUri.ToString();
            } 
            catch (Exception ex)
            {
                return "";
            }
        }
    }

}
