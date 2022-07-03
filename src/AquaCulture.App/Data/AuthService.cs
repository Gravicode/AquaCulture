using Grpc.Net.Client;
using AquaCulture.Models;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaCulture.App.Data
{
    public class AuthService
    {

        IAuth client;

        public AuthService(GrpcChannel channel)
        {


            client = channel.CreateGrpcService<IAuth>();


        }

        public async Task<AuthenticatedUserModel> Login(string username, string password)
        {
            var res = await client.AuthenticateWithUsername(new AuthenticationUserModel() { Username= username, Password= password });
            return res;
        }
    }
}
