using CyberSec.Cryptocurrency.Service.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberSec.Cryptocurrency.Service.Helpers
{
    public static class Converter
    {
         static byte[] ConvertToBytes(this string arg)
        {
            return System.Text.Encoding.UTF8.GetBytes(arg);
        }

        public static byte[] ConvertToByte(this Transaction[] lsTrx)
        {
            var transactionsString = Newtonsoft.Json.JsonConvert.SerializeObject(lsTrx);
            return transactionsString.ConvertToBytes();
        }

        public static string ConvertToString(this Transaction[] lsTrx)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(lsTrx);
        }

        public static string ConvertToHexString(this byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
