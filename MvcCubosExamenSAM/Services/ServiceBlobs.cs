using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using MvcCubosExamenSAM.Models;

namespace MvcCubosExamenSAM.Services
{
    public class ServiceBlobs
    {
        private BlobServiceClient client;

        public ServiceBlobs(BlobServiceClient client)
        {
            this.client = client;
        }

        public async Task<string> GetBlobUriPrivateAsync(string container, string blobName)
        {
            BlobContainerClient containerClient = client.GetBlobContainerClient(container);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            var response = await containerClient.GetPropertiesAsync();
            var properties = response.Value;

            if (properties.PublicAccess == PublicAccessType.None)
            {
                Uri imageUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddSeconds(3600));
                return imageUri.ToString();
            }

            return blobClient.Uri.AbsoluteUri.ToString();
        }

        public async Task<BlobModel> GetBlobUriAsync(string containerName, string blobName)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            BlobModel model = new BlobModel
            {
                Nombre = blobClient.Name,
                Contenedor = blobClient.BlobContainerName,
                Url = blobClient.Uri.AbsoluteUri,
            };
            return model;
        }

        //public async Task<List<BlobModel>> GetBlobsAsync(string containerName)
        //{
        //    BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
        //    List<BlobModel> blobmodels = new List<BlobModel>();
        //    await foreach (BlobItem item in containerClient.GetBlobsAsync())
        //    {
        //        BlobClient blobClient = containerClient.GetBlobClient(item.Name);
        //        BlobModel model = new BlobModel
        //        {
        //            Nombre = blobClient.Name,
        //            Contenedor = blobClient.BlobContainerName,
        //            Url = blobClient.Uri.AbsoluteUri,
        //        };
        //        blobmodels.Add(model);
        //    }
        //    return blobmodels;
        //}

        public async Task UploadBlobAsync(string containerName, string blobName, Stream stream)
        {
            BlobContainerClient containerClient = this.client.GetBlobContainerClient(containerName);
            await containerClient.UploadBlobAsync(blobName, stream);
        }
    }
}
