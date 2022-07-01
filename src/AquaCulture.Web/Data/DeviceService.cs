using AquaCulture.Models;
using Microsoft.EntityFrameworkCore;
using AquaCulture.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaCulture.Web.Data
{
    public class DeviceService : ICrud<Device>
    {
        AquaCultureDB db;

        public DeviceService()
        {
            if (db == null) db = new AquaCultureDB();

        }
        public bool DeleteData(object Id)
        {
            var selData = (db.Devices.Where(x => x.Id == (long)Id).FirstOrDefault());
            db.Devices.Remove(selData);
            db.SaveChanges();
            return true;
        }
        
        public List<Device> FindByKeyword(string Keyword)
        {
            var data = from x in db.Devices
                       where x.Nama.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<Device> GetAllData()
        {
            return db.Devices.ToList();
        }

        public Device GetDataById(object Id)
        {
            return db.Devices.Where(x => x.Id == (long)Id).FirstOrDefault();
        }


        public bool InsertData(Device data)
        {
            try
            {
                db.Devices.Add(data);
                db.SaveChanges();
                return true;
            }
            catch
            {

            }
            return false;

        }



        public bool UpdateData(Device data)
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
            return db.Devices.Max(x => x.Id);
        }
    }

}