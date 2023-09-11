using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Detail_KelompokModel
    {
        [DisplayName("ID Detail Kelompok")]
        public int id_detailkel { get; set; }

        [DisplayName("ID User")]
        [Required]
        public string id_user { get; set; }

        [DisplayName("ID Kelompok")]
        [Required]
        public int id_kel { get; set; }

        public string namakel { get; set; }

        [DisplayName("Nama Anggota")]
        [Required]
        public string nama_anggota { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }
    }
}