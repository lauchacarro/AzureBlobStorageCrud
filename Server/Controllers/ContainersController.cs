using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Threading.Tasks;


namespace AzureBlobStorageCrud.Server.Controllers
{
    [Route("api/[controller]")]
    public class ContainersController : Controller
    {
        private readonly BlobServiceClient _client;

        public ContainersController(BlobServiceClient client)
        {
            _client = client;
        }

        [HttpGet]
        public async IAsyncEnumerable<string> GetAsync()
        {
            await foreach (BlobContainerItem container in _client.GetBlobContainersAsync())
            {
                yield return container.Name;
            }
        }

        [HttpPost]
        public async Task Post([FromBody]string name)
        {
            BlobContainerClient blobContainer = await _client.CreateBlobContainerAsync(name);
        }

        [HttpDelete("{name}")]
        public async Task Delete(string name)
        {
            await _client.DeleteBlobContainerAsync(name);
        }
    }
}
