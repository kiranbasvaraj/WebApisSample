using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApisSample.Extensions
{
    public static class StringExtensions
    {
        public static bool IsBase64Encoded(this string value)
        {
            try
            {
                var data = Convert.FromBase64String(value);
                return (value.Replace(" ", string.Empty).Length % 4 == 0);
            }
            catch
            {
                return false;
            }
        }
    }

}