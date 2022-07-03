using AquaCulture.Data;
using AquaCulture.Models;
using ProtoBuf.Grpc;

namespace AquaCulture.Services.Grpc
{
    public class GrpcUserProfileService : IUserProfile
    {
        UserProfileService svc;
        public GrpcUserProfileService()
        {
            svc = new UserProfileService();
        }


        public Task<List<UserProfile>> FindByKeyword(string Keyword, CallContext context = default)
        {
            var res = svc.FindByKeyword(Keyword);
            return Task.FromResult(res);
        }

        public Task<List<UserProfile>> GetAllData(CallContext context = default)
        {
            try
            {
                var res = svc.GetAllData();
                return Task.FromResult(res);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return Task.FromResult(default(List<UserProfile>));
            }
        }

        public Task<UserProfile> GetDataById(InputCls Id, CallContext context = default)
        {
            var res = svc.GetDataById(long.Parse(Id.Param[0]));
            return Task.FromResult(res);
        }

        public Task<UserProfile> GetItemByEmail(InputCls input, CallContext context = default)
        {
            var res = svc.GetItemByEmail(input.Param[0]);
            return Task.FromResult(res);
        }
         public Task<UserProfile> GetItemByPhone(InputCls input, CallContext context = default)
        {
            var res = svc.GetItemByPhone(input.Param[0]);
            return Task.FromResult(res);
        }


        public Task<OutputCls> GetLastId(CallContext context = default)
        {

            var res = svc.GetLastId();
            return Task.FromResult(new OutputCls() { Data = res.ToString() });
        }

        public Task<OutputCls> GetUserRole(InputCls input, CallContext context = default)
        {
            var res = svc.GetUserRole(input.Param[0]);
            return Task.FromResult(new OutputCls() { Data = res.ToString(), Result=true });
        }

        public Task<OutputCls> IsUserExists(InputCls input, CallContext context = default)
        {
            var res = svc.IsUserExists(input.Param[0]);
            return Task.FromResult(new OutputCls() { Result = res });
        }

        Task<OutputCls> ICrudGrpc<UserProfile>.DeleteData(InputCls Id, CallContext context)
        {
            var res = svc.DeleteData(long.Parse(Id.Param[0]));
            return Task.FromResult(new OutputCls() { Result = res });
        }



        Task<OutputCls> ICrudGrpc<UserProfile>.InsertData(UserProfile data, CallContext context)
        {
            var res = svc.InsertData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }


        Task<OutputCls> ICrudGrpc<UserProfile>.UpdateData(UserProfile data, CallContext context)
        {
            var res = svc.UpdateData(data);
            return Task.FromResult(new OutputCls() { Result = res });
        }

    }
}
