using System;
using System.Collections.Generic;
using System.Text;

namespace AquaCulture.Tools
{
    public class AngkaToWord
    {
        public static string Terbilang(double a)
        {
            return Terbilang((long)a);
        }

        /*
        public static string Terbilang(long a)
        {
            string[] bilangan = new string[] { "", "Satu", "Dua", "Tiga", "Empat", "Lima", "Enam", "Tujuh", "Delapan", "Sembilan", "Sepuluh", "Sebelas" };
            var kalimat = "";
            // 1 - 11
            if (a < 12)
            {
                kalimat = bilangan[a];
            }
            // 12 - 19
            else if (a < 20)
            {
                kalimat = bilangan[a - 10] + " Belas";
            }
            // 20 - 99
            else if (a < 100)
            {
                var utama = a / 10;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 10;
                kalimat = bilangan[depan] + " Puluh " + bilangan[belakang];
            }
            // 100 - 199
            else if (a < 200)
            {
                kalimat = "Seratus " + Terbilang(a - 100);
            }
            // 200 - 999
            else if (a < 1000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 100;
                kalimat = bilangan[depan] + " Ratus " + Terbilang(belakang);
            }
            // 1,000 - 1,999
            else if (a < 2000)
            {
                kalimat = "Seribu " + Terbilang(a - 1000);
            }
            // 2,000 - 9,999
            else if (a < 10000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 1000;
                kalimat = bilangan[depan] + " Ribu " + Terbilang(belakang);
            }
            // 10,000 - 99,999
            else if (a < 100000)
            {
                var utama = a / 100;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 2));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 100,000 - 999,999
            else if (a < 1000000)
            {
                var utama = a / 1000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 3));
                var belakang = a % 1000;
                kalimat = Terbilang(depan) + " Ribu " + Terbilang(belakang);
            }
            // 1,000,000 - 	99,999,999
            else if (a < 100000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));//Substring(0, 4));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 1000000000)
            {
                var utama = a / 1000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 4));
                var belakang = a % 1000000;
                kalimat = Terbilang(depan) + " Juta " + Terbilang(belakang);
            }
            else if (a < 10000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 100000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 1000000000000)
            {
                var utama = a / 1000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000;
                kalimat = Terbilang(depan) + " Milyar " + Terbilang(belakang);
            }
            else if (a < 10000000000000)
            {
                var utama = a / 10000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 10000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }
            else if (a < 100000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 2));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 1000000000000000)
            {
                var utama = a / 1000000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 3));
                var belakang = a % 1000000000000;
                kalimat = Terbilang(depan) + " Triliun " + Terbilang(belakang);
            }

            else if (a < 10000000000000000)
            {
                var utama = a / 1000000000000000;
                var depan = Convert.ToInt64(utama.ToString().Substring(0, 1));
                var belakang = a % 1000000000000000;
                kalimat = Terbilang(depan) + " Kuadriliun " + Terbilang(belakang);
            }

            var pisah = kalimat.Split(' ');
            List<string> full = new List<string>();// = [];
            for (var i = 0; i < pisah.Length; i++)
            {
                if (pisah[i] != "") { full.Add(pisah[i]); }
            }
            return CombineTerbilang(full.ToArray());// full.Concat(' '); .join(' ');
        }
        static string CombineTerbilang(string[] arr)
        {
            return string.Join(" ", arr);
        }*/
        private static string Terbilang(long angka)
        {
            string strterbilang = "";
            //membuat array untuk mengubah 1 - 11 menjadi terbilang
            string[] a = { "", "satu", "dua", "tiga", "empat", "lima", "enam", "tujuh", "delapan", "sembilan", "sepuluh", "sebelas" };
            if (angka < 12) { strterbilang = " " + a[angka]; }
            else if (angka < 20) { strterbilang = Terbilang(angka - 10) + " belas"; }
            else if (angka < 100) { strterbilang = Terbilang(angka / 10) + " puluh" + Terbilang(angka % 10); }
            else if (angka < 200) { strterbilang = " seratus" + Terbilang(angka - 100); }
            else if (angka < 1000)
            { strterbilang = Terbilang(angka / 100) + " ratus" + Terbilang(angka % 10); }
            else if (angka < 2000) { strterbilang = " seribu" + Terbilang(angka - 1000); }
            else if (angka < 1000000) { strterbilang = Terbilang(angka / 1000) + " ribu" + Terbilang(angka % 1000); }
            else if (angka < 1000000000) { strterbilang = Terbilang(angka / 1000000) + " juta" + Terbilang(angka % 1000000); }
            //menghilangkan multiple space
            strterbilang = System.Text.RegularExpressions.Regex.Replace(strterbilang, @"^\s+|\s+$", " ");
            //mengembalikan hasil terbilang
            return strterbilang;
        }
    }
}
