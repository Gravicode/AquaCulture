using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using AquaCulture.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Services.Helpers
{
    public class AzureBlobHelper
    {
        public async static Task<bool> UploadFile(string fileName, byte[] Data)
        {
            try
            {
                string storageConnection = AppConstants.BlobConn;
                CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(storageConnection);

                //create a block blob 
                CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();

                //create a container CloudBlobContainer 
                var cloudBlobContainer = cloudBlobClient.GetContainerReference("portal-ub");

                //create a container if it is not already exists

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                {

                    await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

                }

                //string imageName = $"{shortid.ShortId.Generate(false, false, 5)}_{fileName}";//+ Path.GetExtension(fileName);

                //get Blob reference

                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
                //cloudBlockBlob.Properties.ContentType = imageToUpload.ContentType;

                await cloudBlockBlob.UploadFromByteArrayAsync(Data, 0, Data.Length);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
