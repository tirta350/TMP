using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class MatkulModel
    {
        [DisplayName("ID Matkul")]
        public int id_matkul { get; set; }

        [DisplayName("ID Prodi")]
        [Required]
        public int id_prodi { get; set; }

        public string prodinama { get; set; }

        [DisplayName("Nama Matkul")]
        [Required]
        public string nama_matkul { get; set; }

        [DisplayName("Dosen Pengampu")]
        [Required]
        public string dosen_pengampu { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }
    }
}