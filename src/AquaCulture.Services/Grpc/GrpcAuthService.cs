using AquaCulture.Data;
using AquaCulture.Models;
using AquaCulture.Tools;
using ProtoBuf.Grpc;

namespace AquaCulture.Services.Grpc
{
    public class GrpcAuthService : IAuth
    {
        UserProfileService svc;
        public GrpcAuthService()
        {
            svc = new UserProfileService();
        }
        public Task<AuthenticatedUserModel> AuthenticateWithApiKey(AuthenticationModel data, CallContext context = default)
        {
            var Gagal = true;
            if (data.ApiKey == AppConstants.ApiKey)
            {
                Gagal = false;
            }
            var res = new AuthenticatedUserModel() { AccessToken = AquaCulture.Tools.RandomGenerator.StaticGenerator.RandomNumber(10), ExpiredDate = DateHelper.GetLocalTimeNow().AddDays(1), TokenType = "random", Username = "admin" };
            if (Gagal)
            {
                res.Username = String.Empty;
                res.ExpiredDate = DateHelper.GetLocalTimeNow().AddDays(-1);
                res.AccessToken = String.Empty;
            }
            return Task.FromResult(res);
        }

        public Task<AuthenticatedUserModel> AuthenticateWithUsername(AuthenticationUserModel data, CallContext context = default)
        {
            var Nama = data.Username;
            var Password = data.Password;
            var isAuthenticate = false;
            var usr = svc.GetItemByEmail(Nama);
            if (usr != null)
            {
                var enc = new Encryption();
                var pass = enc.Decrypt(usr.Password);
                isAuthenticate = pass == Password;
            
            }
            var res = new AuthenticatedUserModel() { AccessToken = AquaCulture.Tools.RandomGenerator.StaticGenerator.RandomNumber(10), ExpiredDate = DateHelper.GetLocalTimeNow().AddDays(1), TokenType = "random", Username = Nama };
            if (!isAuthenticate)
            {
                res.Username = String.Empty;
                res.ExpiredDate = DateHelper.GetLocalTimeNow().AddDays(-1);
                res.AccessToken = String.Empty;
            }
            return Task.FromResult(res);
        }
    }
}
