using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Transaksi_Acc_Step
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ProyekModel> getAllData(UserModel user) // ini buat ngambil semua data prodi
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek WHERE pic LIKE '" + namaUser + "' AND status='3'", con);
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

        public Detail_ProyekModel getData(int id)
        {
            Detail_ProyekModel detail_Proyek = new Detail_ProyekModel();
            SqlCommand cmd = new SqlCommand("Select * from proyek where id_proyek like @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {

                detail_Proyek.id_proyek = Convert.ToInt32(dr[0].ToString());
                detail_Proyek.nama_kegiatan = dr[1].ToString();
                detail_Proyek.nama_proyek = dr[2].ToString();
            }
            else
            {

            }
            dr.Close();
            con.Close();
            return detail_Proyek;
        }

        public Detail_ProyekModel getDataProgress(int id)
        {
            Detail_ProyekModel detail_Proyek = new Detail_ProyekModel();
            SqlCommand cmd = new SqlCommand("Select dp.id_detail as id_detail, p.tanggal_mulai as tanggal_mulai, p.target as target, " +
                "dp.id_proyek as id_proyek, dp.nama_kegiatan as nama_kegiatan, dp.tahap as tahap," +
                "dp.komentar as komentar, dp.problem_identification as problem_identification, dp.corrective_action as corrective_action, " +
                "dp.status as status, p.nama_proyek as nama_proyek" +
                " from detail_proyek as dp JOIN proyek as p ON dp.id_proyek = p.id_proyek " +
                "WHERE dp.id_detail = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                detail_Proyek.id_detail = Convert.ToInt32(dr["id_detail"].ToString());
                detail_Proyek.tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("d MMMM yyyy");
                detail_Proyek.target = Convert.ToDateTime(dr["target"]).ToString("d MMMM yyyy");
                detail_Proyek.id_proyek = Convert.ToInt32(dr["id_proyek"].ToString());
                detail_Proyek.nama_kegiatan = dr["nama_kegiatan"].ToString();
                detail_Proyek.tahap = dr["tahap"].ToString();
                detail_Proyek.komentar = dr["komentar"].ToString();
                detail_Proyek.problem_identification = dr["problem_identification"].ToString();
                detail_Proyek.corrective_action = dr["corrective_action"].ToString();
                detail_Proyek.status = Convert.ToInt32(dr["status"].ToString());
                detail_Proyek.nama_proyek = dr["nama_proyek"].ToString();
            }
            else
            {

            }

            dr.Close();
            con.Close();
            return detail_Proyek;
        }

        public int getCountProgress(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as total FROM detail_proyek WHERE id_proyek = @id", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            Int32 count = (Int32)cmd.ExecuteScalar();
            con.Close();

            return count;
        }

        public int getSuccessStatus(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as total FROM detail_proyek WHERE id_proyek = @id AND status = '3'", con);
            cmd.Parameters.AddWithValue("@id", id);

            con.Open();
            Int32 count = (Int32)cmd.ExecuteScalar();
            con.Close();

            return count;
        }
    }
}