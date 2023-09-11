using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TMP.Models
{
    public class UserModel
    {
        [DisplayName("Id User")]
        public int id_user { get; set; }

        [DisplayName("Nama User")]
        [Required]
        public string nama_user { get; set; }

        [DisplayName("Username")]
        [Required]
        public string username { get; set; }

        [DisplayName("Password")]
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        //[DisplayName("Tempat Lahir")]
        //[Required]
        //public string tempat_lahir { get; set; }

        //[DisplayName("Tanggal Lahir")]
        //[Required]
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //public string tanggal_lahir { get; set; }

        [DisplayName("Email")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [DisplayName("Nomor Telepon")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string nomor_tlp { get; set; }

        [DisplayName("Alamat")]
        [Required]
        public string alamat { get; set; }

        [DisplayName("Jenis Kelamin")]
        [Required]
        public string jenis_kelamin { get; set; }

        [DisplayName("Role")]
        [Required]
        public string role { get; set; }

        [DisplayName("Kode Token")]
        [Required]
        public string kode_token { get; set; }

        [DisplayName("Status")]
        [Required]
        public int status { get; set; }

        [DisplayName("Nama Kelompok")]
        [Required]
        public string nama_kel { get; set; }

        [DisplayName("Nama")]
        [Required]
        public string nama { get; set; }

        [DisplayName("NPK")]
        [Required]
        public string npk { get; set; }

        [DisplayName("Jabatan")]
        [Required]
        public string jabatan { get; set; }

        public string nim { get; set; }

    }
}