using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace AquaCulture.Tools
{
    public class FileWriter
    {
        public static bool DeleteFile(String FileName)
        {
            try
            {
                FileInfo TheFile = new FileInfo(FileName);
                if (TheFile.Exists)
                {
                    System.IO.File.Delete(FileName);
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }

            catch (FileNotFoundException ex)
            {
                Logs.WriteLog("Gagal delete file - " + ex.Message + ", " + ex.StackTrace);
                return false;
            }
            catch (Exception ex)
            {
                Logs.WriteLog("Gagal delete file - " + ex.Message + ", " + ex.StackTrace);
                return false;
            }
            return true;
        }

        public static string ReadFile(String FileName)
        {

            FileInfo TheFile = new FileInfo(FileName);
            string str = "";
            if (!TheFile.Exists)
            {
                return "";
            }

            str = File.ReadAllText(FileName);

            return str;
        }
        public static void AppendToFile(String teks, String Path)
        {
            try
            {
                StreamWriter SW;
                SW = System.IO.File.AppendText(Path);
                SW.WriteLine(teks);
                SW.Flush();
                //Console.WriteLine("Text Appended Successfully");
                SW.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }
        }
        public static void WriteFile(String teks, String Path)
        {
            try
            {
                File.WriteAllText(Path, teks);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                //Console.WriteLine("Executing finally block.");
            }
        }
    }
}
