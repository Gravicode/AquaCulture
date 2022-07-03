using AquaCulture.Models;
using Microsoft.EntityFrameworkCore;
using AquaCulture.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Data
{
    public class GatewayService : ICrud<Gateway>
    {
        AquaCultureDB db;

        public GatewayService()
        {
            if (db == null) db = new AquaCultureDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Gateways.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Gateways.Remove(selData);
            db.SaveChanges();
            return true;
        }

        public List<Gateway> FindByKeyword(string Keyword)
        {
            var data = from x in db.Gateways
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Gateway> GetAllData()
        {
            return db.Gateways.ToList();
        }

        public Gateway GetDataById(object Id)
        {
            return db.Gateways.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Gateway data)
        {
            try
            {
                db.Gateways.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Gateway data)
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
            return db.Gateways.Max(x => x.Id);
        }
    }

}