using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using PeerApp.Azlayer.Interface;

namespace PeerApp.Azlayer.Implementation
{
    public class AzStorageOperations : IAzStorageOperations
    {
        public async Task BlobUploadAsync(IFormFile file, string fileName)
        {
            try
            {
                var containerClient = await BlobContainerClient();
                BlobClient blobClient = containerClient.GetBlobClient(fileName);
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream);
                }
            }
            catch (Exception ex) {
                throw ex;
            }
        }

        private static async  Task<BlobContainerClient> BlobContainerClient()
        {
            var blobServiceClient = new BlobServiceClient(
            new Uri("https://aisearchlearn001.blob.core.windows.net/"),
            new DefaultAzureCredential());

            //Create a unique name for the container
            string containerName = "webcamimages";

            // Create the container and return a container client object
            var containerClient= blobServiceClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();
            return containerClient;
        }
    }
}
