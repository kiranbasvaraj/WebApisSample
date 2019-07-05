using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApisSample.Models
{
    public class SpeakersModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string About { get; set; }
        public string SpeechId { get; set; }
        public string SpeechDateTime { get; set; }
        public string SpeakerId { get; set; }
    }
}