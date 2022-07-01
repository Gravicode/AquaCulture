using AquaCulture.Models;
using Microsoft.EntityFrameworkCore;
using AquaCulture.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Web.Data
{
    public class StreamRawDataService : ICrud<StreamRawData>
    {
        AquaCultureDB db;

        public StreamRawDataService()
        {
            if (db == null) db = new AquaCultureDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.StreamRawDatas.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.StreamRawDatas.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<StreamRawData> FindByKeyword(string Keyword)
        {
            var data = from x in db.StreamRawDatas
                       where x.RawData.Contains(Keyword) 
                       select x;
            return data.ToList();
        }

        public List<StreamRawData> GetAllData()
        {
            return db.StreamRawDatas.ToList();
        }

        public StreamRawData GetDataById(object Id)
        {
            return db.StreamRawDatas.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(StreamRawData data)
        {
            try
            {
                db.StreamRawDatas.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(StreamRawData data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                /*
                if (sel != null)
                {
                    sel.Nama = data.Nama;
                    sel.Keterangan = data.Keterangan;
                    sel.Tanggal = data.Tanggal;
                    sel.DocumentUrl = data.DocumentUrl;
                    sel.StreamUrl = data.StreamUrl;
                    return true;

                }*/
                return true;
            }
            catch
            {

            }
            return false;
        }

        public long GetLastId()
        {
            return db.StreamRawDatas.Max(x => x.Id);
        }
    }

}