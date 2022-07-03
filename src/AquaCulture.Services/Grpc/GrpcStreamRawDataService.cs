using AquaCulture.Data;
using AquaCulture.Models;
using ProtoBuf.Grpc;

namespace AquaCulture.Services.Grpc
{
    public class GrpcStreamRawDataService : IStreamRawData
    {
        StreamRawDataService svc;
        public GrpcStreamRawDataService()
        {
            svc = new StreamRawDataService();
        }


        public Task<List<StreamRawData>> FindByKeyword(string Keyword, CallContext context = default)
        {
            var res = svc.FindByKeyword(Keyword);
            return Task.FromResult(res);
        }

        public Task<List<StreamRawData>> GetAllData(CallContext context = default)
        {
            try
            {
                var res = svc.GetAllData();
                return Task.FromResult(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Task.FromResult(default(List<StreamRawData>));
            }
        }

        public Task<StreamRawData> GetDataById(InputCls Id, CallContext context = default)
        {
            var res = svc.GetDataById(long.Parse(Id.Param[0]));
            return Task.FromResult(res);
        }

        public Task<OutputCls> GetLastId(CallContext context = default)
        {

            var res = svc.GetLastId();
            return Task.FromResult(new OutputCls() { Data = res.ToString() });
        }



        Task<OutputCls> ICrudGrpc<StreamRawData>.DeleteData(InputCls Id, CallContext context)
        {
            var res = svc.DeleteData(long.Parse(Id.Param[0]));
            return Task.FromResult(new OutputCls() { Result = res });
        }



        Task<OutputCls> ICrudGrpc<StreamRawData>.InsertData(StreamRawData data, CallContext context)
        {
            var res = svc.InsertData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }


        Task<OutputCls> ICrudGrpc<StreamRawData>.UpdateData(StreamRawData data, CallContext context)
        {
            var res = svc.UpdateData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }

    }
}
