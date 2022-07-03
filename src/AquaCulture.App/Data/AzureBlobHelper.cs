//using Microsoft.Azure.Storage;
//using Microsoft.Azure.Storage.Blob;
using Azure.Storage.Blobs;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.App.Data
{
  

    public class AzureBlobHelper
    {
        BlobServiceClient client;
        BlobContainerClient containerClient;
        //BlobClient blobClient;
        public AzureBlobHelper()
        {
            Setup();

            //string imageName = $"{shortid.ShortId.Generate(false, false, 5)}_{fileName}";//+ Path.GetExtension(fileName);

        }
        //public CloudBlobContainer cloudBlobContainer { get; set; }
        async void Setup()
        {
            try
            {
                string storageConnection = AppConstants.BlobConn;

                string containerName = "aqua-culture";

                client = new BlobServiceClient(storageConnection);

                containerClient = client.GetBlobContainerClient(containerName);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
          
        

            //CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

            ////create a block blob 
            //CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

            ////create a container CloudBlobContainer 
            //cloudBlobContainer = cloudBlobClient.GetContainerReference("ngaji-online");

            ////create a container if it is not already exists

            //if (await cloudBlobContainer.CreateIfNotExistsAsync())
            //{

            //    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });

            //}
        }
        public async Task<bool> UploadFile(string fileName, byte[] Data)
        {
            try
            {

                //get Blob reference

                //CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                //await cloudBlockBlob.UploadFromByteArrayAsync(Data, 0, Data.Length);

                using MemoryStream memoryStream = new MemoryStream(Data);
                await containerClient.UploadBlobAsync(fileName, memoryStream);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
