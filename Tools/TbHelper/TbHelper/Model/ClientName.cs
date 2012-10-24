using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Infrastructure.Crosscutting.Utility;

namespace TbHelper.Model
{
   public class ClientName
   {
        public ClientName()
        {}
        public ClientName(DataRow dr)
        {
            Name = Util.Get<string>(dr, "Name"); 
        }
       public string Name { get; set; }
   }
}
