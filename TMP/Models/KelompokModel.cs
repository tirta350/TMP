using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class KelompokModel
    {
        [DisplayName("ID Kelompok")]
        public int id_kel { get; set; }

        [DisplayName("Nama Kelompok")]
        [Required]
        public string nama_kel { get; set; }

        [DisplayName("Tahun")]
        [Required]
        public string tahun { get; set; }

        [DisplayName("Status")]
        public int status { get; set; }

    }
}