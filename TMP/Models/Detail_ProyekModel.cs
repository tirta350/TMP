using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Models
{
    public class Detail_ProyekModel
    {
        public List<ProyekModel> listproyek { get; set; }

        public ProyekModel proyek { get; set; }

        public MatkulModel matkul { get; set; }

        [DisplayName("ID Detail ")]
        public int? id_detail { get; set; }

        [DisplayName("ID Proyek")]
        [Required]
        public int id_proyek { get; set; }

        [DisplayName("Nama Kegiatan")]
        [Required]
        public string nama_kegiatan { get; set; }

        [DisplayName("Tahap")]
        public string tahap { get; set; }

        [DisplayName("Komentar")]
        [AllowHtml]
        public string komentar { get; set; }

        [DisplayName("Problem Identification")]
        public string problem_identification { get; set; }

        [DisplayName("Corrective Action")]
        public string corrective_action { get; set; }


        [DisplayName("Status")]
        [Required]
        public int status { get; set; }

        [Display(Name = "File")]
        public string pry_file { get; set; }
        public HttpPostedFileBase UploadFileOSP { get; set; }

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

        [DisplayName("Nama Proyek")]
        public string nama_proyek { get; set; }

        [DisplayName("Nomor Telepon")]
        public String nomor_tlp { get; set; }
    }
}