using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Laporan_Proyek
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ProyekModel> getAllData() // ini buat ngambil semua data prodi
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

        public List<Laporan_Proyek_Model> getAllDataLaporan() // ini buat ngambil semua data prodi
        {
            List<Laporan_Proyek_Model> laporan = new List<Laporan_Proyek_Model>();

            SqlCommand cmd = new SqlCommand("Select p.nama_proyek as nama_proyek, p.pic as pic, p.progress as progress, p.target as target, dk.nama_kegiatan as nama_kegiatan, dk.problem_identification as problem_identification, dk.corrective_action as corrective_action, dk.status as status" +
                " from proyek as p JOIN detail_proyek as dk ON p.id_proyek = dk.id_proyek", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                laporan.Add(new Laporan_Proyek_Model()
                {
                    nama_proyek = dr["nama_proyek"].ToString(),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    nama_kegiatan = dr["nama_kegiatan"].ToString(),
                    problem_identification =dr["problem_identification"].ToString(),
                    corrective_action = dr["corrective_action"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return laporan;
        }

        public List<Laporan_Proyek_Model> getAllDataLaporanByDateRange(DateTime tanggalAwalDate, DateTime tanggalAkhirDate)
        {
            List<Laporan_Proyek_Model> laporan = new List<Laporan_Proyek_Model>();

            // Use parameterized query to avoid SQL injection
            string query = "SELECT p.nama_proyek AS nama_proyek, p.pic AS pic, p.progress AS progress, p.target AS target, dk.nama_kegiatan AS nama_kegiatan, dk.problem_identification AS problem_identification, dk.corrective_action AS corrective_action, dk.status AS status " +
                           "FROM proyek AS p JOIN detail_proyek AS dk ON p.id_proyek = dk.id_proyek " +
                           "WHERE p.target >= @tanggalAwal AND p.target <= @tanggalAkhir";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@tanggalAwal", tanggalAwalDate);
            cmd.Parameters.AddWithValue("@tanggalAkhir", tanggalAkhirDate);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                laporan.Add(new Laporan_Proyek_Model()
                {
                    nama_proyek = dr["nama_proyek"].ToString(),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    nama_kegiatan = dr["nama_kegiatan"].ToString(),
                    problem_identification = dr["problem_identification"].ToString(),
                    corrective_action = dr["corrective_action"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            }
            dr.Close();
            con.Close();
            return laporan;
        }

        public List<ProyekModel> getAllDataDosen(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
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

        public List<Laporan_Proyek_Model> getAllDataLaporanByDateRangeDosen(UserModel user,DateTime tanggalAwalDate, DateTime tanggalAkhirDate)
        {
            List<Laporan_Proyek_Model> laporan = new List<Laporan_Proyek_Model>();
            string namaUser = user.nama_user;

            // Use parameterized query to avoid SQL injection
            string query = "SELECT p.nama_proyek AS nama_proyek, p.pic AS pic, p.progress AS progress, p.target AS target, dk.nama_kegiatan AS nama_kegiatan, dk.problem_identification AS problem_identification, dk.corrective_action AS corrective_action, dk.status AS status " +
                           "FROM proyek AS p JOIN detail_proyek AS dk ON p.id_proyek = dk.id_proyek " +
                           "WHERE p.target >= @tanggalAwal AND p.target <= @tanggalAkhir AND p.pic = '" + namaUser + "'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@tanggalAwal", tanggalAwalDate);
            cmd.Parameters.AddWithValue("@tanggalAkhir", tanggalAkhirDate);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                laporan.Add(new Laporan_Proyek_Model()
                {
                    nama_proyek = dr["nama_proyek"].ToString(),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    nama_kegiatan = dr["nama_kegiatan"].ToString(),
                    problem_identification = dr["problem_identification"].ToString(),
                    corrective_action = dr["corrective_action"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            }
            dr.Close();
            con.Close();
            return laporan;
        }

        public List<ProyekModel> getAllDataMahasiswa(UserModel user)
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select * from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel JOIN detail_kelompok as dk on k.id_kel = dk.id_kel WHERE p.status != 0 AND dk.nama_anggota LIKE '" + namaUser + "'", con);
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

        public List<Laporan_Proyek_Model> getAllDataLaporanByDateRangeMahasiswa(UserModel user, DateTime tanggalAwalDate, DateTime tanggalAkhirDate)
        {
            List<Laporan_Proyek_Model> laporan = new List<Laporan_Proyek_Model>();
            string namaUser = user.nama_user;

            // Use parameterized query to avoid SQL injection
            string query = "SELECT p.nama_proyek AS nama_proyek, p.pic AS pic, p.progress AS progress, p.target AS target, dk.nama_kegiatan AS nama_kegiatan, dk.problem_identification AS problem_identification, dk.corrective_action AS corrective_action, dk.status AS status " +
               "FROM proyek AS p " +
               "JOIN detail_proyek AS dk ON p.id_proyek = dk.id_proyek " +
               "JOIN kelompok AS k ON p.id_kel = k.id_kel " +
               "JOIN detail_kelompok AS dkel ON k.id_kel = dkel.id_kel " +
               "WHERE p.target >= @tanggalAwal AND p.target <= @tanggalAkhir AND dkel.nama_anggota LIKE '"+namaUser+"'";

            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@tanggalAwal", tanggalAwalDate);
            cmd.Parameters.AddWithValue("@tanggalAkhir", tanggalAkhirDate);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                laporan.Add(new Laporan_Proyek_Model()
                {
                    nama_proyek = dr["nama_proyek"].ToString(),
                    pic = dr["pic"].ToString(),
                    progress = Convert.ToInt32(dr["progress"].ToString()),
                    target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy"),
                    nama_kegiatan = dr["nama_kegiatan"].ToString(),
                    problem_identification = dr["problem_identification"].ToString(),
                    corrective_action = dr["corrective_action"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            }
            dr.Close();
            con.Close();
            return laporan;
        }
    }
}