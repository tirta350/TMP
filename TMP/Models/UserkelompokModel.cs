using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class UserKelompokModel
    {
        public List<UserModel> listuser { get; set; }
        public KelompokModel kelompok { get; set; }
        public ProyekModel proyek { get; set; }
        public List<MatkulModel> listmatkul { get; set; }
        public List<ProyekModel> listproyek { get; set; }


        public int i { get; set; }
    }
}