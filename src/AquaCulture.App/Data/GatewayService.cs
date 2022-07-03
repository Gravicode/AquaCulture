using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AquaCulture.Models;

namespace AquaCulture.App.Data
{
    public class GatewayService : ICrud<Gateway>
    {
        IGateway client;

        public GatewayService(GrpcChannel channel)
        {


            client = channel.CreateGrpcService<IGateway>();


        }
        public async Task<bool> DeleteData(long Id)
        {
            var res = await client.DeleteData(new InputCls() { Param = new string[] { Id.ToString() } });
            return res.Result;
        }

        public async Task<List<Gateway>> FindByKeyword(string Keyword)
        {
            var data = await client.FindByKeyword(Keyword);
            return data;
        }

        public async Task<List<Gateway>> GetAllData()
        {
            var data = await client.GetAllData();
            return data;
        }



        public async Task<Gateway> GetDataById(long Id)
        {
            var res = await client.GetDataById(new InputCls() { Param = new string[] { Id.ToString() } });
            return res;
        }


        public async Task<bool> InsertData(Gateway data)
        {
            try
            {
                var res = await client.InsertData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<bool> Update(Gateway data)
        {
            try
            {
                var res = await client.UpdateData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }



        public async Task<bool> UpdateData(Gateway data)
        {
            try
            {
                var res = await client.UpdateData(data);
                return res.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        public async Task<long> GetLastId()
        {
            var res = await client.GetLastId();
            return long.Parse(res.Data);
        }
    }
}
