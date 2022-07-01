using System;

namespace AquaCulture.Tools
{
    public  class NumberGen
    {
        static Random rnd = new Random(Environment.TickCount);
        public static string GenerateNumber(int length=10)
        {
            var NumStr = "";
            for(int i = 0; i < length; i++)
            {
                NumStr += rnd.Next(0, 10);
            }
            return NumStr;
        }
    }
}
