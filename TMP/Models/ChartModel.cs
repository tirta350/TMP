using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ChartModel
    {
        public string nama_proyek { get; set; }
        public int progress { get; set; }
        public string pic { get; set; }
        public Dashboard_AdminModel dashboard_admin { get; set; }
    }
}