using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Transaksi_Upload_Progress
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ProyekModel> getAllData(UserModel user) // ini buat ngambil semua data prodi
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek as p join kelompok as k on p.id_kel = k.id_kel join " +
                "detail_kelompok as dk on k.id_kel = dk.id_kel where dk.id_user =" + idUser + "AND p.status='3'", con);
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

        public List<Detail_MatkulModel> getAllDataDetail(int id) // ini buat ngambil semua data matkul
        {
            List<Detail_MatkulModel> detail_matkul = new List<Detail_MatkulModel>();

            SqlCommand cmd = new SqlCommand("Select * from detail_matkul where id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                detail_matkul.Add(new Detail_MatkulModel()
                {
                    id_detailmatkul = Convert.ToInt32(dr["id_detailmatkul"].ToString()),
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_matkul = Convert.ToInt32(dr["id_matkul"].ToString()),
                    nama_matkul = dr["nama_matkul"].ToString()
                });
            };
            dr.Close();
            con.Close();
            return detail_matkul;
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

        public string getDataContact(int id)
        {
            string nomor_tlp = null; // Initialize the variable to store nomor_tlp

            SqlCommand cmd = new SqlCommand("Select p.id_proyek as id_proyek, p.nama_proyek as nama_proyek, u.nomor_tlp as nomor_tlp from proyek as p join [user] as u on p.pic LIKE u.nama_user" +
                " where p.id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read()) // Use if instead of while, as we only expect one result
            {
                nomor_tlp = dr["nomor_tlp"].ToString(); // Store the value of nomor_tlp into the variable
            }

            dr.Close();
            con.Close();

            return nomor_tlp; // Return the nomor_tlp as a string
        }

        public string getDataNamaProyek(int id)
        {
            string nama_proyek = null; // Initialize the variable to store nomor_tlp

            SqlCommand cmd = new SqlCommand("Select p.id_proyek as id_proyek, p.nama_proyek as nama_proyek, u.nomor_tlp as nomor_tlp from proyek as p join [user] as u on p.pic LIKE u.nama_user" +
                " where p.id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read()) // Use if instead of while, as we only expect one result
            {
                nama_proyek = dr["nama_proyek"].ToString(); // Store the value of nomor_tlp into the variable
            }

            dr.Close();
            con.Close();

            return nama_proyek; // Return the nomor_tlp as a string
        }

        public string getDataNamaKelompok(int id)
        {
            string nama_kel = null; // Initialize the variable to store nomor_tlp

            SqlCommand cmd = new SqlCommand("Select k.nama_kel from proyek as p join kelompok as k on k.id_kel = p.id_kel" +
                " where p.id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read()) // Use if instead of while, as we only expect one result
            {
                nama_kel = dr["nama_kel"].ToString(); // Store the value of nomor_tlp into the variable
            }

            dr.Close();
            con.Close();

            return nama_kel; // Return the nomor_tlp as a string
        }

        public string getDataNamaKegiatan(int? id)
        {
            string nama_kegiatan = null; // Initialize the variable to store nomor_tlp

            SqlCommand cmd = new SqlCommand("Select nama_kegiatan from detail_proyek" +
                " where id_detail = @id_detail", con);
            cmd.Parameters.AddWithValue("@id_detail", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read()) // Use if instead of while, as we only expect one result
            {
                nama_kegiatan = dr["nama_kegiatan"].ToString(); // Store the value of nomor_tlp into the variable
            }

            dr.Close();
            con.Close();

            return nama_kegiatan; // Return the nomor_tlp as a string
        }
    }
}