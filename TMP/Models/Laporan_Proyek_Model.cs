using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Laporan_Proyek_Model
    {
        [DisplayName("Nama Proyek")]
        [Required]
        public string nama_proyek { get; set; }

        [DisplayName("PIC")]
        [Required]
        public string pic { get; set; }

        [DisplayName("Progress")]
        [Required]
        public int progress { get; set; }

        [DisplayName("Target")]
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public string target { get; set; }

        [DisplayName("Nama Kegiatan")]
        [Required]
        public string nama_kegiatan { get; set; }

        [DisplayName("Problem Identification")]
        public string problem_identification { get; set; }

        [DisplayName("Corrective Action")]
        public string corrective_action { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }
    }
}