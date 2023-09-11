using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Dashboard_AdminModel
    {
        public string nama_proyek { get; set; }
        public string pic { get; set; }
        public int progress { get; set; }
        public string tahun { get; set; }
    }
}