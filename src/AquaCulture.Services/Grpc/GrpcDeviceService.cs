using AquaCulture.Data;
using AquaCulture.Models;
using ProtoBuf.Grpc;

namespace AquaCulture.Services.Grpc
{
    public class GrpcDeviceService : IDevice
    {
        DeviceService svc;
        public GrpcDeviceService()
        {
            svc = new DeviceService();
        }


        public Task<List<Device>> FindByKeyword(string Keyword, CallContext context = default)
        {
            var res = svc.FindByKeyword(Keyword);
            return Task.FromResult(res);
        }

        public Task<List<Device>> GetAllData(CallContext context = default)
        {
            try
            {
                var res = svc.GetAllData();
                return Task.FromResult(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Task.FromResult(default(List<Device>));
            }
        }

        public Task<Device> GetDataById(InputCls Id, CallContext context = default)
        {
            var res = svc.GetDataById(long.Parse(Id.Param[0]));
            return Task.FromResult(res);
        }

        public Task<OutputCls> GetLastId(CallContext context = default)
        {

            var res = svc.GetLastId();
            return Task.FromResult(new OutputCls() { Data = res.ToString() });
        }



        Task<OutputCls> ICrudGrpc<Device>.DeleteData(InputCls Id, CallContext context)
        {
            var res = svc.DeleteData(long.Parse(Id.Param[0]));
            return Task.FromResult(new OutputCls() { Result = res });
        }



        Task<OutputCls> ICrudGrpc<Device>.InsertData(Device data, CallContext context)
        {
            var res = svc.InsertData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }


        Task<OutputCls> ICrudGrpc<Device>.UpdateData(Device data, CallContext context)
        {
            var res = svc.UpdateData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }

    }
}
