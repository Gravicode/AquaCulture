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
    public class StreamRawDataService : ICrud<StreamRawData>
    {
        IStreamRawData client;

        public StreamRawDataService(GrpcChannel channel)
        {


            client = channel.CreateGrpcService<IStreamRawData>();


        }
        public async Task<bool> DeleteData(long Id)
        {
            var res = await client.DeleteData(new InputCls() { Param = new string[] { Id.ToString() } });
            return res.Result;
        }

        public async Task<List<StreamRawData>> FindByKeyword(string Keyword)
        {
            var data = await client.FindByKeyword(Keyword);
            return data;
        }

        public async Task<List<StreamRawData>> GetAllData()
        {
            var data = await client.GetAllData();
            return data;
        }



        public async Task<StreamRawData> GetDataById(long Id)
        {
            var res = await client.GetDataById(new InputCls() { Param = new string[] { Id.ToString() } });
            return res;
        }


        public async Task<bool> InsertData(StreamRawData data)
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

        public async Task<bool> Update(StreamRawData data)
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



        public async Task<bool> UpdateData(StreamRawData data)
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
