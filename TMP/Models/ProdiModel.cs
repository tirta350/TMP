using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class ProdiModel
    {
        [DisplayName("Id Prodi")]
        public int id_prodi { get; set; }
        
        [DisplayName("Nama Prodi")]
        [Required]
        public string nama_prodi { get; set; }

        [DisplayName("status")]
        public int status { get; set; }
    }
}