using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace webForwarder_Report.Models
{
    public class TaiPDFRequest
    {
        public long ID { get; set; }
        public long IDDanhMucDonVi { get; set; }
        public long IDDanhMucChungTu { get; set; }
        public string So { get; set; }
        public long IDDanhMucDaiLy { get; set; }
        //public bool IsKMG { get; set; }
    }
}