using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TMP.Models;

namespace TMP.Models
{
    public class Dashboard_Admin
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Matkul _matkul = new Matkul();
        Kelompok _kel = new Kelompok();

        public int GetTotalProjects()
        {
            int totalProjects = 0;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TotalProjects FROM proyek WHERE status != 0", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                totalProjects = Convert.ToInt32(dr["TotalProjects"]);
            }
            dr.Close();
            con.Close();

            return totalProjects;
        }

        public int GetOnProgressProjects()
        {
            int onprogressProjects = 0;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS OnProgressProjects FROM proyek WHERE status NOT IN (0, 1)", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                onprogressProjects = Convert.ToInt32(dr["OnProgressProjects"]);
            }
            dr.Close();
            con.Close();

            return onprogressProjects;
        }

        public int GetNeedAttentionProject()
        {
            int NeedAttentionProject = 0;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS NeedAttentionProject FROM proyek WHERE status NOT IN (0, 1, 2) AND progress <= 50", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                NeedAttentionProject = Convert.ToInt32(dr["NeedAttentionProject"]);
            }
            dr.Close();
            con.Close();

            return NeedAttentionProject;
        }

        public List<Dashboard_AdminModel> getAllData() // ini buat ngambil semua data prodi
        {
            List<Dashboard_AdminModel> pic = new List<Dashboard_AdminModel>();

            SqlCommand cmd = new SqlCommand("Select pic from proyek WHERE status != 0 GROUP by pic", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                pic.Add(new Dashboard_AdminModel()
                {
                    pic = dr["pic"].ToString(),
                });
            };
            dr.Close();
            con.Close();
            return pic;
        }

        public List<Dashboard_AdminModel> getDataTahunDosen(UserModel user) // ini buat ngambil semua data tahun dosen
        {
            List<Dashboard_AdminModel> tahun = new List<Dashboard_AdminModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select YEAR(p.tanggal_mulai) AS tahun from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel WHERE p.status != 0" +
                "AND p.pic LIKE '" + namaUser + "' GROUP by YEAR(p.tanggal_mulai)", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                tahun.Add(new Dashboard_AdminModel()
                {
                    tahun = dr["tahun"].ToString(),
                });
            };
            dr.Close();
            con.Close();
            return tahun;
        }

        public List<Dashboard_AdminModel> getDataTahunMahasiswa(UserModel user) // ini buat ngambil semua data tahun mahasiswa
        {
            List<Dashboard_AdminModel> tahun1 = new List<Dashboard_AdminModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd1 = new SqlCommand("Select YEAR(p.tanggal_mulai) AS tahun from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel JOIN detail_kelompok as dk on k.id_kel = dk.id_kel WHERE p.status != 0 AND dk.nama_anggota LIKE '" + namaUser + "' GROUP BY YEAR(p.tanggal_mulai)", con);
            con.Open();
            SqlDataReader dr = cmd1.ExecuteReader();
            while (dr.Read())
            {
                tahun1.Add(new Dashboard_AdminModel()
                {
                    tahun = dr["tahun"].ToString(),
                });
            };
            dr.Close();
            con.Close();
            return tahun1;
        }

        public List<ProyekModel> EntireProject()
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status != 0", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> OnProgressProject()
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status NOT IN (0, 1)", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> NeedAttentionProject()
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status NOT IN (0, 1, 2) AND progress <= 50", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public int GetTotalProjectsLecturer(UserModel user)
        {
            int totalProjectsLecturer = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TotalProjectsLecturer FROM proyek WHERE status != 0 AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                totalProjectsLecturer = Convert.ToInt32(dr["TotalProjectsLecturer"]);
            }
            dr.Close();
            con.Close();

            return totalProjectsLecturer;
        }

        public int GetOnProgressProjectsLecturer(UserModel user)
        {
            int onprogressProjectsLecturer = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS OnProgressProjectsLecturer FROM proyek WHERE status NOT IN (0, 1) AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                onprogressProjectsLecturer = Convert.ToInt32(dr["OnProgressProjectsLecturer"]);
            }
            dr.Close();
            con.Close();

            return onprogressProjectsLecturer;
        }

        public int GetNeedAttentionProjectLecturer(UserModel user)
        {
            int NeedAttentionProjectLecturer = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS NeedAttentionProjectLecturer FROM proyek WHERE status NOT IN (0, 1, 2) AND progress <= 50 AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                NeedAttentionProjectLecturer = Convert.ToInt32(dr["NeedAttentionProjectLecturer"]);
            }
            dr.Close();
            con.Close();

            return NeedAttentionProjectLecturer;
        }

        public List<ProyekModel> EntireProjectLecturer(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status != 0 AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> OnProgressProjectLecturer(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status NOT IN (0, 1, 2) AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> NeedAttentionProjectLecturer(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select * from proyek WHERE status NOT IN (0, 1, 2) AND progress <= 50 AND pic LIKE '" + namaUser + "'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public int GetTotalProjectsStudent(UserModel user)
        {
            int totalProjectsStudent = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS TotalProjectsStudent FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
                "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status != '0'", con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                totalProjectsStudent = Convert.ToInt32(dr["TotalProjectsStudent"]);
            }
            dr.Close();
            con.Close();

            return totalProjectsStudent;
        }

        public int GetOnProgressProjectsStudent(UserModel user)
        {
            int onprogressProjectsStudent = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS OnProgressProjectsStudent FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
                "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status NOT IN (0, 1)", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                onprogressProjectsStudent = Convert.ToInt32(dr["OnProgressProjectsStudent"]);
            }
            dr.Close();
            con.Close();

            return onprogressProjectsStudent;
        }

        public int GetNeedAttentionProjectStudent(UserModel user)
        {
            int NeedAttentionProjectStudent = 0;
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS NeedAttentionProjectStudent FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
               "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status NOT IN (0, 1, 2) AND progress <= 50", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                NeedAttentionProjectStudent = Convert.ToInt32(dr["NeedAttentionProjectStudent"]);
            }
            dr.Close();
            con.Close();

            return NeedAttentionProjectStudent;
        }

        public List<ProyekModel> EntireProjectStudent(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
                "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status != '0'", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> OnProgressProjectStudent(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
                "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status NOT IN (0, 1)", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

        public List<ProyekModel> NeedAttentionProjectStudent(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek AS p JOIN kelompok AS k ON p.id_kel = k.id_kel JOIN " +
               "detail_kelompok AS dk ON k.id_kel = dk.id_kel WHERE dk.id_user = " + idUser + " AND p.status NOT IN (0, 1) AND progress <= 50", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy"),
                    semester = Convert.ToInt32(dr["semester"].ToString()),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }
    }
}   