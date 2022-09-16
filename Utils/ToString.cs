using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class ToString
    {
        public static string[] ToStrings(this string obj, char chars)
        {
           return obj.Split(new char[] { chars }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
