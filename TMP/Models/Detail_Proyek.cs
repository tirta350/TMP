using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Detail_Proyek
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Matkul _matkul = new Matkul();
        Kelompok _kel = new Kelompok();

        public List<ProyekModel> getAllData(UserModel user) // ini buat ngambil semua data prodi
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek as p join kelompok as k on p.id_kel = k.id_kel join " +
                "detail_kelompok as dk on k.id_kel = dk.id_kel where dk.id_user =" + idUser + "AND p.status='2'", con);
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

        public List<ProyekModel> getAllData1(UserModel user) // ini buat ngambil semua data prodi yang status nya belum ada detail_proyek nya
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            int idUser = user.id_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek as p join kelompok as k on p.id_kel = k.id_kel join " +
                "detail_kelompok as dk on k.id_kel = dk.id_kel where dk.id_user =" + idUser + "AND p.status='1'", con);
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

        public List<Detail_ProyekModel> getAllDataDetail(int id) // ini buat ngambil semua data matkul
        {
            List<Detail_ProyekModel> detail_proyek = new List<Detail_ProyekModel>();

            SqlCommand cmd = new SqlCommand("Select * from detail_proyek where id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                detail_proyek.Add(new Detail_ProyekModel()
                {
                    id_detail = Convert.ToInt32(dr["id_detail"].ToString()),
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    nama_kegiatan = dr["nama_kegiatan"].ToString(),
                    tahap = dr["tahap"].ToString(),
                    komentar = dr["komentar"].ToString(),
                    problem_identification = dr["problem_identification"].ToString(),
                    corrective_action = dr["corrective_action"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString())
                });
            };
            dr.Close();
            con.Close();
            return detail_proyek;
        }

        public Detail_ProyekModel getData(int id)
        {
            Detail_ProyekModel detail_proyek = new Detail_ProyekModel();
            SqlCommand cmd = new SqlCommand("Select dk.id_detail as id_detail, dk.id_proyek as id_proyek, dk.nama_kegiatan as nama_kegiatan," +
                "dk.tahap as tahap, dk.komentar as komentar, dk.problem_identification as problem_identification," +
                "dk.corrective_action as corrective_action, dk.status as status, p.nama_proyek as nama_proyek from detail_proyek as dk join proyek as p on dk.id_proyek = p.id_proyek" +
                " where dk.id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {

                detail_proyek.id_proyek = Convert.ToInt32(dr["id_proyek"].ToString());
                detail_proyek.nama_proyek = dr["nama_proyek"].ToString();
                //detail_proyek.id_detail = Convert.ToInt32(dr["id_detail"].ToString());
                //detail_proyek.id_proyek = Convert.ToInt32(dr["id_proyek"].ToString());
                //detail_proyek.nama_kegiatan = dr["nama_kegiatan"].ToString();

            }
            else
            {

            }
            dr.Close();
            con.Close();
            return detail_proyek;
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
    }
}