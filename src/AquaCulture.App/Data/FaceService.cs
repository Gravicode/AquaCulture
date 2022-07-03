using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.Face;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Color = System.Drawing.Color;
using Font = System.Drawing.Font;
using Point = System.Drawing.Point;
using SizeF = System.Drawing.SizeF;

namespace AquaCulture.App.Data
{
    public class FaceService
    {
        HttpClient client;
        // Add your Face subscription key to your environment variables.
        private static string subscriptionKey = AppConstants.FACE_SUBSCRIPTION_KEY;
        // Add your Face endpoint to your environment variables.
        private static string faceEndpoint = AppConstants.FACE_ENDPOINT;

        private readonly IFaceClient faceClient = new FaceClient(
            new ApiKeyServiceClientCredentials(subscriptionKey),
            new System.Net.Http.DelegatingHandler[] { });

        // The list of detected faces.
        private IList<DetectedFace> faceList;
        // The list of descriptions for the detected faces.
        private string[] faceDescriptions;
        // The resize factor for the displayed image.
        private double resizeFactor;

        private const string defaultStatusBarText =
            "Place the mouse pointer over a face to see the face description.";

        public FaceService()
        {
            if (client == null) client = new HttpClient();
            if (Uri.IsWellFormedUriString(faceEndpoint, UriKind.Absolute))
            {
                faceClient.Endpoint = faceEndpoint;
            }
            else
            {
               //error uri
            }
        }

        // Uploads the image file and calls DetectWithStreamAsync.
        public async Task<(IList<DetectedFace> faces,System.Drawing.Image img)> UploadAndDetectFaces(byte[] imageFileData)
        {
            // The list of Face attributes to return.
            IList<FaceAttributeType?> faceAttributes =
                new FaceAttributeType?[]
                {
            FaceAttributeType.Gender, FaceAttributeType.Age,
            FaceAttributeType.Smile, FaceAttributeType.Emotion,
            FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            // Call the Face API.
            try
            {
                // The second argument specifies to return the faceId, while
                // the third argument specifies not to return face landmarks.
                var ms = new MemoryStream(imageFileData);
                IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithStreamAsync(
                        ms, true, false, faceAttributes);
                ms = new MemoryStream(imageFileData);
                var img = System.Drawing.Image.FromStream(ms);
                var boxedImg = DrawBoundingBox((Bitmap)img, faceList);
                    return (faceList,boxedImg);
                
            }
            // Catch and display Face API errors.
            catch (APIErrorException f)
            {
                //MessageBox.Show(f.Message);
                return (new List<DetectedFace>(),null);
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error");
                return (new List<DetectedFace>(), null);
            }
        }
        public async Task<(IList<DetectedFace> faces, System.Drawing.Image img)> DetectFacesFromUrl(string ImageUrl)
        {
            // The list of Face attributes to return.
            IList<FaceAttributeType?> faceAttributes =
                new FaceAttributeType?[]
                {
            FaceAttributeType.Gender, FaceAttributeType.Age,
            FaceAttributeType.Smile, FaceAttributeType.Emotion,
            FaceAttributeType.Glasses, FaceAttributeType.Hair
                };

            // Call the Face API.
            try
            {
                // The second argument specifies to return the faceId, while
                // the third argument specifies not to return face landmarks.
                IList<DetectedFace> faceList =
                    await faceClient.Face.DetectWithUrlAsync(
                        ImageUrl, true, false, faceAttributes);
                var imageFileData = await client.GetByteArrayAsync(ImageUrl);
                var ms = new MemoryStream(imageFileData);
                var img = System.Drawing.Image.FromStream(ms);
                var boxedImg = DrawBoundingBox((Bitmap)img, faceList);
                return (faceList, boxedImg);


            }
            // Catch and display Face API errors.
            catch (APIErrorException f)
            {
                //MessageBox.Show(f.Message);
                return (new List<DetectedFace>(), null);
            }
            // Catch and display all other errors.
            catch (Exception e)
            {
                //MessageBox.Show(e.Message, "Error");
                return (new List<DetectedFace>(), null);
            }
        }

        public System.Drawing.Image DrawBoundingBox(Bitmap imageData, IList<DetectedFace> faces)
        {
            System.Drawing.Image image = imageData;//Image.FromFile(imageFilePath);
            var originalHeight = image.Height;
            var originalWidth = image.Width;
            var count = 1;
            foreach (var box in faces)
            {
               
                    //// process output boxes
                    var x = (uint)Math.Max(box.FaceRectangle.Left, 0);
                    var y = (uint)Math.Max(box.FaceRectangle.Top, 0);
                    var width = (uint)Math.Min(originalWidth - x, box.FaceRectangle.Width);
                    var height = (uint)Math.Min(originalHeight - y, box.FaceRectangle.Height);

                    // fit to current image size
                    //x = ((uint)originalWidth * x / originalWidth);
                    //y = ((uint)originalHeight * y / originalHeight);
                    //width = (uint)originalWidth * width / originalWidth;
                    //height = (uint)originalHeight * height / ImageSettings.imageHeight;

                    using (Graphics thumbnailGraphic = Graphics.FromImage(image))
                    {
                        thumbnailGraphic.CompositingQuality = CompositingQuality.HighQuality;
                        thumbnailGraphic.SmoothingMode = SmoothingMode.HighQuality;
                        thumbnailGraphic.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        // Define Text Options
                        Font drawFont = new Font("Arial", 8, FontStyle.Bold);
                        SizeF size = thumbnailGraphic.MeasureString(count.ToString(), drawFont);
                        SolidBrush fontBrush = new SolidBrush(Color.Black);
                        Point atPoint = new Point((int)x, (int)y - (int)size.Height - 1);

                        // Define BoundingBox options
                        Pen pen = new Pen(Color.LightGreen, 3.2f);
                        SolidBrush colorBrush = new SolidBrush(Color.LightGreen);

                        // Draw text on image 
                        thumbnailGraphic.FillRectangle(colorBrush, (int)x, (int)(y - size.Height - 1), (int)size.Width, (int)size.Height);
                        thumbnailGraphic.DrawString(count.ToString(), drawFont, fontBrush, atPoint);

                        // Draw bounding box on image
                        thumbnailGraphic.DrawRectangle(pen, x, y, width, height);
                        count++;
                    }
                
            }
            return image;
        }
    }
}
