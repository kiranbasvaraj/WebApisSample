using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;

namespace WebApisSample.Services
{
    public class JsonFormator
    {

        public static JsonMediaTypeFormatter GetFormator()
        {
                var formatter = new JsonMediaTypeFormatter();
                var json = formatter.SerializerSettings;
                json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
                json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                json.Formatting = Newtonsoft.Json.Formatting.Indented;
                return formatter;
        }
    }
}