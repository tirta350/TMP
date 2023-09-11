using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ProyekModel
    {

        [DisplayName("ID Proyek")]
        public int id_proyek { get; set; }

        [DisplayName("ID Kelompok")]
        [Required]
        public int id_kel { get; set; }

        public string namakel { get; set; }

        [DisplayName("Nama Proyek")]
        [Required]
        public string nama_proyek { get; set; }

        [DisplayName("Target")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string target { get; set; }

        [DisplayName("Tanggal Mulai")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string tanggal_mulai { get; set; }

        [DisplayName("Semester")]
        [Required]
        public int semester { get; set; }

        [DisplayName("PIC")]
        [Required]
        public string pic { get; set; }

        [DisplayName("Progress")]
        [Required]
        public int progress { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }
    }
}