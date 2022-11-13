using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAPI.Entities
{
    public class SOInfo
    {
        public CusInfo CustInfo { get; set; }
        public List<ct64> ListItems { get; set; }
    }
}