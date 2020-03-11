using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace AzureBlobStorageCrud.Server.Controllers
{
    [Route("api/containers/{containerName}/[controller]")]
    public class BlobsController : Controller
    {

        private readonly BlobServiceClient _client;

        public BlobsController(BlobServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async IAsyncEnumerable<string> Get(string containerName)
        {
            BlobContainerClient container = _client.GetBlobContainerClient(containerName);
            await foreach (var blob in container.GetBlobsAsync())
            {
                yield return blob.Name;
            }
        }

        [HttpGet("{name}")]
        public async Task<FileResult> Get(string containerName, string name)
        {
            BlobContainerClient container = _client.GetBlobContainerClient(containerName);
            BlobClient blob = container.GetBlobClient(name);
            Response<BlobDownloadInfo> downloadInfo = await blob.DownloadAsync();

            return File(downloadInfo.Value.Content, downloadInfo.Value.ContentType, name);
        }

        [HttpPost]
        public void Post(string containerName, IFormFile file)
        {
            BlobContainerClient container = _client.GetBlobContainerClient(containerName);
            container.UploadBlobAsync(file.FileName, file.OpenReadStream());
        }

        [HttpDelete("{name}")]
        public async Task Delete(string containerName, string name)
        {
            BlobContainerClient container = _client.GetBlobContainerClient(containerName);
            BlobClient blob = container.GetBlobClient(name);
            await blob.DeleteAsync();
        }
    }
}
