using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TMP.Models
{
    public class Detail_MatkulModel
    {
        [DisplayName("ID Detail Matkul")]
        public int id_detailmatkul { get; set; }

        [DisplayName("ID Proyek")]
        [Required]
        public int id_proyek { get; set; }

        [DisplayName("ID Mata Kuliah")]
        [Required]
        public int id_matkul { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }

        public string matkuldosennama { get; set; }

        [DisplayName("Nama Mata Kuliah")]
        [Required]
        public string nama_matkul{ get; set; }
    }
}