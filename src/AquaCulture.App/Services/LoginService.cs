using Grpc.Net.Client;
using AquaCulture.App.Data;
using AquaCulture.App.Helpers;
using AquaCulture.Models;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AquaCulture.App.Services
{
    public interface ILoginService
    {
        Task<AuthenticatedUserModel> Authenticate(string username, string password);
        Task<AuthenticatedUserModel> Authenticate(string apiKey);
    }
    public class LoginService : ILoginService
    {
        private readonly GrpcChannel _channel;
        private readonly ISyncMemoryStorageService _localStorage;
        IAuth client;
        public LoginService(GrpcChannel channel, ISyncMemoryStorageService localStorage)
        {
            _channel = channel;
            _localStorage = localStorage;
            client = _channel.CreateGrpcService<IAuth>();
        }

        public async Task<AuthenticatedUserModel> Authenticate(string username, string password)
        {
            var authUser = new AuthenticationUserModel
            {
                Username = username,
                Password = password
            };
            var res = await client.AuthenticateWithUsername(authUser);

           
            if (!string.IsNullOrEmpty(res.Username))
            {
               
                await _localStorage.SetItem(AppConstants.Authentication, res);
                //AppConstant.ApiSecret = data.AccessToken;
                return res;
            }
            return null;
        }

        public async Task<AuthenticatedUserModel> Authenticate(string apiKey)
        {
            var authKey = new AuthenticationModel
            {
                ApiKey = apiKey
            };
            var res = await client.AuthenticateWithApiKey(authKey);


            if (!string.IsNullOrEmpty(res.Username))
            {

                await _localStorage.SetItem(AppConstants.Authentication, res);
                //AppConstant.ApiSecret = data.AccessToken;
                return res;
            }
            return null;
        }
    }
}
