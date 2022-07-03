using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AquaCulture.Models;
using GemBox.Spreadsheet;
using AquaCulture.Tools;

namespace AquaCulture.App.Data
{
    public class UserProfileService : ICrud<UserProfile>
    {
        IUserProfile client;

        public UserProfileService(GrpcChannel channel)
        {


            client = channel.CreateGrpcService<IUserProfile>();


        }

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
        public async Task<byte[]> ExportToExcelAsync()
        {
            // If using Professional version, put your serial key below.
            //SpreadsheetInfo.SetLicense(AppConstants.GemLic);

            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Anggota");
            var datas = await GetAllData();
            int row = 1;

            var styleHeader = new CellStyle();
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
            styleHeader.Font.Weight = ExcelFont.BoldWeight;
            styleHeader.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
            styleHeader.WrapText = true;
            styleHeader.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);



            worksheet.Cells[0, 0].Value = "No";
            worksheet.Cells[0, 1].Value = "Nama";
            worksheet.Cells[0, 2].Value = "Alamat";
            worksheet.Cells[0, 3].Value = "Telepon";
            worksheet.Cells[0, 4].Value = "Email";
            worksheet.Cells[0, 5].Value = "Aktif";
            worksheet.Cells[0, 6].Value = "KTP";
            worksheet.Cells[0, 7].Value = "Username";
            worksheet.Cells[0, 8].Value = "Operator";


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
            style.Font.Color = SpreadsheetColor.FromName(ColorName.Black);
            style.WrapText = true;
            style.Borders.SetBorders(MultipleBorders.Left | MultipleBorders.Right | MultipleBorders.Top | MultipleBorders.Bottom, SpreadsheetColor.FromName(ColorName.Black), LineStyle.Thin);

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
        public async Task<bool> DeleteData(long Id)
        {
            var res = await client.DeleteData(new InputCls() { Param = new string[] { Id.ToString() } });
            return res.Result;
        } 
        
        public async Task<bool> IsUserExists(string Username)
        {
            var res = await client.IsUserExists(new InputCls() { Param = new string[] { Username.ToString() } });
            return res.Result;
        }
        
        public async Task<UserProfile> GetItemByEmail(string Email)
        {
            var res = await client.GetItemByEmail(new InputCls() { Param = new string[] { Email } });
            return res;
        } 
        
      
        
        public async Task<UserProfile> GetItemByPhone(string Phone)
        {
            var res = await client.GetItemByPhone(new InputCls() { Param = new string[] { Phone } });
            return res;
        }  
        
        public async Task<Roles> GetUserRole(string Username)
        {
            var res = await client.GetUserRole(new InputCls() { Param = new string[] {Username } });
            Roles role;
            Enum.TryParse<Roles>(res.Data, out role);
            return  role;
        }

        public async Task<List<UserProfile>> FindByKeyword(string Keyword)
        {
            var data = await client.FindByKeyword(Keyword);
            return data;
        }

        public async Task<List<UserProfile>> GetAllData()
        {
            var data = await client.GetAllData();
            return data;
        }



        public async Task<UserProfile> GetDataById(long Id)
        {
            var res = await client.GetDataById(new InputCls() { Param = new string[] { Id.ToString() } });
            return res;
        }


        public async Task<bool> InsertData(UserProfile data)
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

        public async Task<bool> Update(UserProfile data)
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



        public async Task<bool> UpdateData(UserProfile data)
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
