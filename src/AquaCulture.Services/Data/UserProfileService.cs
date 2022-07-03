using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AquaCulture.Models;
using GemBox.Spreadsheet;
using System.Drawing;
using AquaCulture.Tools;

namespace AquaCulture.Data
{
    public class UserProfileService : ICrud<UserProfile>
    {
        AquaCultureDB db;
        public UserProfileService()
        {
            if (db == null) db = new AquaCultureDB();
            //db.Database.EnsureCreated();
        }
        public bool DeleteData(object Id)
        {
            if (Id is long FID)
            {
                var data = from x in db.UserProfiles
                           where x.Id == FID
                           select x;
                foreach (var item in data)
                {
                    db.UserProfiles.Remove(item);
                }
                db.SaveChanges();
                return true;
            }
            return false;
        }

        public UserProfile GetItemByEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email)) return null;
            var selItem = db.UserProfiles.Where(x => x.Email.ToLower() == Email.ToLower()).FirstOrDefault();
            return selItem;
        }
        public Roles GetUserRole(string Email)
        {
            var selItem = db.UserProfiles.Where(x => x.Username == Email).FirstOrDefault();
            return selItem.Role;
        }

        public UserProfile GetUserByEmail(string Email)
        {
            var selItem = db.UserProfiles.Where(x => x.Username == Email).FirstOrDefault();
            return selItem;
        }
        public UserProfile GetItemByPhone(string Phone)
        {
            var selItem = db.UserProfiles.Where(x => x.Phone.ToLower() == Phone.ToLower()).FirstOrDefault();
            return selItem;
        }
        public List<UserProfile> FindByKeyword(string Keyword)
        {
            var data = from x in db.UserProfiles
                       where x.Email.Contains(Keyword) || x.FullName.Contains(Keyword)
                       select x;
            return data.ToList();
        }

        public List<UserProfile> GetAllData()
        {
            var data = from x in db.UserProfiles
                       select x;
            return data.ToList();
        }

        public UserProfile GetDataById(object Id)
        {
            if (Id is long FID)
            {
                var data = from x in db.UserProfiles
                           where x.Id == FID
                           select x;
                return data.FirstOrDefault();
            }
            return default;
        }

        public long GetLastId()
        {
            var lastId = db.UserProfiles.OrderByDescending(x => x.Id).FirstOrDefault();
            return lastId.Id + 1;
        }
        public bool IsUserExists(string Email)
        {
            if (string.IsNullOrEmpty(Email)) return true;
            //if (db.UserProfiles.Count() <= 0 ) return false;
            var exists = db.UserProfiles.Any(x => x.Username.ToLower() == Email.ToLower());
            return exists;
        }
        public bool InsertData(UserProfile data)
        {
            try
            {
                db.UserProfiles.Add(data);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }

        }

        public bool UpdateData(UserProfile data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                return true;
            }
            catch (Exception)
            {
                return false;


            }

        }
        #region Excel
        public static List<UserProfile> ReadExcel(string PathFile)
        {
            try
            {
                Encryption enc = new Encryption();
                var DefaultPass = enc.Encrypt("354jaya");
                var DataUserProfile = new List<UserProfile>();
                //SpreadsheetInfo.SetLicense(AppConstants.GemLic);

                if (!File.Exists(PathFile)) return null;
                var workbook = ExcelFile.Load(PathFile);


                // Iterate through all worksheets in an Excel workbook.
                var worksheet = workbook.Worksheets[0];
                var counter = 0;
                // Iterate through all rows in an Excel worksheet.
                foreach (var row in worksheet.Rows)
                {
                    if (counter > 0)
                    {
                        var newUserProfile = new UserProfile();
                        //newUserProfile.KK = row.Cells[0].Value?.ToString();
                        //newUserProfile.NoUrut = int.Parse(row.Cells[1].Value?.ToString());

                        newUserProfile.FullName = row.Cells[1].Value?.ToString().Trim();
                        newUserProfile.Alamat = row.Cells[2].Value?.ToString().Trim();
                        newUserProfile.Phone = row.Cells[3].Value?.ToString().Trim();
                        newUserProfile.Email = row.Cells[4].Value?.ToString().Trim();
                        newUserProfile.Aktif = row.Cells[5].Value?.ToString().Trim() == "Ya" ? true : false;
                        newUserProfile.KTP = row.Cells[6].Value?.ToString().Trim();
                        newUserProfile.Username = string.IsNullOrEmpty(row.Cells[10].Value?.ToString().Trim()) ? MailHelper.GenerateEmailFromName(newUserProfile.FullName.Trim(), "AquaCulture.online") : row.Cells[10].Value?.ToString().Trim();
                        if (string.IsNullOrEmpty(newUserProfile.Email))
                        {
                            newUserProfile.Email = newUserProfile.Username;
                        }
                        newUserProfile.Role = row.Cells[11].Value?.ToString().Trim() == "Ya" ? Roles.Operator : Roles.User;
                        newUserProfile.Password = DefaultPass;
                        DataUserProfile.Add(newUserProfile);
                    }
                    counter++;


                }

                return DataUserProfile;
            }
            catch
            {
                return null;
            }

        }

        public OutputCls ImportData(List<UserProfile> NewData)
        {
            var output = new OutputCls();
            try
            {
                Encryption enc = new Encryption();
                var currentData = db.UserProfiles.ToList();
                foreach (var item in NewData)
                {
                    //UserProfile? existing = null;
                    //foreach(var x in currentData)
                    //{
                    //    if(x.Username == item.Username)
                    //    {
                    //        existing = x; break;
                    //    }
                    //}
                    var existing = currentData.Where(x => x.Username.Equals(item.Username, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();
                    if (existing == null)
                    {
                        item.Password = enc.Encrypt(AppConstants.DefaultPass);
                        db.UserProfiles.Add(item);
                    }
                    else
                    {
                        existing.FullName = item.FullName;
                        existing.Alamat = item.Alamat;
                        existing.Phone = item.Phone;
                        existing.Email = item.Email;
                        existing.Aktif = item.Aktif;
                        existing.KTP = item.KTP;
                   
                        //existing.Username =   item.Username  ;
                        existing.Role = item.Role;
                    }
                }
                db.SaveChanges();
                output.Result = true;

            }
            catch (Exception ex)
            {
                output.Result = false;
                output.Message = ex.ToString();
                Console.WriteLine(ex);

            }
            return output;
        }

        public byte[] ExportToExcel()
        {
            // If using Professional version, put your serial key below.
            //SpreadsheetInfo.SetLicense(AppConstants.GemLic);

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Anggota");
            var datas = GetAllData();
            int row = 1;

            var styleHeader = new CellStyle();
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
            styleHeader.Font.Weight = ExcelFont.BoldWeight;
            styleHeader.Font.Color = Color.Black;
            styleHeader.WrapText = true;
            styleHeader.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);



            worksheet.Cells[0, 0].Value = "No";
            worksheet.Cells[0, 1].Value = "Nama";
            worksheet.Cells[0, 2].Value = "Alamat";
            worksheet.Cells[0, 3].Value = "Telepon";
            worksheet.Cells[0, 4].Value = "Email";
            worksheet.Cells[0, 5].Value = "Aktif";
            worksheet.Cells[0, 6].Value = "KTP";
            worksheet.Cells[0, 7].Value = "Username";
            worksheet.Cells[0, 8].Value = "Pengurus";


            worksheet.Cells[0, 0].Style = styleHeader;
            worksheet.Cells[0, 1].Style = styleHeader;
            worksheet.Cells[0, 2].Style = styleHeader;
            worksheet.Cells[0, 3].Style = styleHeader;
            worksheet.Cells[0, 4].Style = styleHeader;
            worksheet.Cells[0, 5].Style = styleHeader;
            worksheet.Cells[0, 6].Style = styleHeader;
            worksheet.Cells[0, 7].Style = styleHeader;
            worksheet.Cells[0, 8].Style = styleHeader;


            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.NormalWeight;
            style.Font.Color = Color.Black;
            style.WrapText = true;
            style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, Color.Black, LineStyle.Thin);

            foreach (var item in datas)
            {



                worksheet.Cells[row, 0].Value = row;
                worksheet.Cells[row, 1].Value = item.FullName;
                worksheet.Cells[row, 2].Value = item.Alamat;
                worksheet.Cells[row, 3].Value = item.Phone;
                worksheet.Cells[row, 4].Value = item.Email;
                worksheet.Cells[row, 5].Value = item.Aktif ? "Ya" : "Tidak";
                worksheet.Cells[row, 6].Value = item.KTP;
                worksheet.Cells[row, 7].Value = item.Username?.ToString();
                worksheet.Cells[row, 8].Value = item.Role == Roles.Operator ? "Ya" : "Tidak";


                worksheet.Cells[row, 0].Style = style;
                worksheet.Cells[row, 1].Style = style;
                worksheet.Cells[row, 2].Style = style;
                worksheet.Cells[row, 3].Style = style;
                worksheet.Cells[row, 4].Style = style;
                worksheet.Cells[row, 5].Style = style;
                worksheet.Cells[row, 6].Style = style;
                worksheet.Cells[row, 7].Style = style;
                worksheet.Cells[row, 8].Style = style;


                row++;
            }
            var tmpfile = Path.GetTempFileName() + ".xlsx";

            workbook.Save(tmpfile);
            return File.ReadAllBytes(tmpfile);
        }
        #endregion
    }
}

