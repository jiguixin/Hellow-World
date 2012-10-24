using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Utility;

namespace TbHelper.Model
{
    public class Url
    {
        public Url()
        {
        }

        public Url(DataRow dr)
        { 
            WebSite = Util.Get<string>(dr, "WebSite");
        }
         
        public string WebSite { get; set; }
    }
}
