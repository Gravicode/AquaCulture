using AquaCulture.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace AquaCulture.App.Data
{
    public class AppConstants
    {
        public static string DefaultPass = "123qweasd";
        public const string NameKey = "Nama";
        public static string GrpcUrl = "";
        public static string LaporanStatistikUrl = "";
        //key-pair
        public const string Authentication = "auth";

        public const int FACE_WIDTH = 180;
        public const int FACE_HEIGHT = 135;
        public const string FACE_SUBSCRIPTION_KEY = "a068e60df8254cc5a187e3e8c644f316";
        public const string FACE_ENDPOINT = "https://southeastasia.api.cognitive.microsoft.com/";

        public static string SQLConn = "";
        public static string BlobConn { get; set; }
        public const string GemLic = "EDWG-SKFA-D7J1-LDQ5";
        public static string RedisCon { set; get; }
       
    }
}
