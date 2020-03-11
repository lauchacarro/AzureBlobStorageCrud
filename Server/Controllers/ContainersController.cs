using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        // GET: api/<controller>
        [HttpGet]
        public async IAsyncEnumerable<string> GetAsync()
        {
            await foreach (BlobContainerItem container in _client.GetBlobContainersAsync())
            {
                yield return container.Name;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public async Task Post([FromBody]string name)
        {
            BlobContainerClient blobContainer = await _client.CreateBlobContainerAsync(name);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
            throw new InvalidOperationException();
        }

        // DELETE api/<controller>/5
        [HttpDelete("{name}")]
        public async Task Delete(string name)
        {
            await _client.DeleteBlobContainerAsync(name);
        }
    }
}
