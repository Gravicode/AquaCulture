using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Web.Data
{
    public interface ICrud<T> where T:class
    {
        bool InsertData(T data);
        bool UpdateData(T data);
        List<T> GetAllData();

        List<T> FindByKeyword(string Keyword);
        T GetDataById(object Id);
        bool DeleteData(object Id);
        long GetLastId();
    }
}
