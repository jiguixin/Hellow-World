using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Infrastructure.Crosscutting.Utility;

namespace TbHelper.Model
{
    public class DataList
    {
        public DataList()
        {}
        public DataList(DataRow dr)
        {
            FullName = Util.Get<string>(dr, "FullName");
            SearchKey = Util.Get<string>(dr, "SearchKey");
        }
        public string FullName { get; set; }
        public string SearchKey { get; set; }  
    }
}
