using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace TMP.Models
{
    public class Proyek
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Matkul _matkul = new Matkul();
        Kelompok _kel = new Kelompok();

        public List<ProyekModel> getAllData(UserModel user) // ini buat ngambil semua data proyek yang berkaitan dengan user yang login
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

        public List<Point> getAllDataDiagram() // ini buat ngambil semua data prodi
        {
            List<Point> point = new List<Point>();

            SqlCommand cmd = new SqlCommand("Select * from proyek", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                point.Add(new Point()
                {
                    x = dr[2].ToString(),
                    y = Convert.ToInt32(dr[7].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return point;
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
                MatkulModel matkul = _matkul.getData(Convert.ToInt32(dr["id_matkul"]));

                detail_matkul.Add(new Detail_MatkulModel()
                {
                    id_detailmatkul = Convert.ToInt32(dr["id_detailmatkul"].ToString()),
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    id_matkul = Convert.ToInt32(dr["id_matkul"].ToString()),
                    matkuldosennama = matkul.dosen_pengampu,
                    nama_matkul = dr["nama_matkul"].ToString()
                    
                });
            };
            dr.Close();
            con.Close();
            return detail_matkul;
        }

        public ProyekModel getData(int id)
        {
            ProyekModel proyek = new ProyekModel();
            SqlCommand cmd = new SqlCommand("Select * from proyek where id_proyek like @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {
                KelompokModel kel = _kel.getData(Convert.ToInt32(dr["id_kel"]));

                proyek.id_proyek = Convert.ToInt32(dr["id_proyek"].ToString());
                proyek.id_kel = Convert.ToInt32(dr["id_kel"].ToString());
                proyek.namakel = kel.nama_kel;
                proyek.nama_proyek = dr["nama_proyek"].ToString();
                proyek.target = Convert.ToDateTime(dr["target"]).ToString("dd-MM-yyyy");
                proyek.tanggal_mulai = Convert.ToDateTime(dr["tanggal_mulai"]).ToString("dd-MM-yyyy");
                proyek.semester = Convert.ToInt32(dr["semester"].ToString());
                proyek.pic = dr["pic"].ToString();
                proyek.progress = Convert.ToInt32(dr["progress"].ToString());
                proyek.status = Convert.ToInt32(dr["status"].ToString());

            }
            else
            {

            }
            dr.Close();
            con.Close();
            return proyek;
        }

        public string[] getDataContacts(int id)
        {
            List<string> nomor_tlpList = new List<string>(); // Initialize the list to store nomor_tlp

            SqlCommand cmd = new SqlCommand("SELECT TOP 1 p.id_proyek AS id_proyek, p.nama_proyek AS nama_proyek, u.nomor_tlp AS nomor_tlp " +
                                     "FROM proyek AS p " +
                                     "JOIN kelompok AS k ON p.id_kel = k.id_kel " +
                                     "JOIN detail_kelompok AS dk ON k.id_kel = dk.id_kel " +
                                     "JOIN [user] AS u ON dk.id_user = u.id_user " +
                                     "WHERE p.id_proyek = @id_proyek", con);
            cmd.Parameters.AddWithValue("@id_proyek", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read()) // Use while to read multiple results
            {
                string nomor_tlp = dr["nomor_tlp"].ToString(); // Get the phone number from the reader
                nomor_tlpList.Add(nomor_tlp); // Add the phone number to the list
            }

            dr.Close();
            con.Close();

            return nomor_tlpList.ToArray(); // Convert the list to an array and return it
        }

        public string getDataNamaProyek(int id)
        {
            string nama_proyek = null; // Initialize the variable to store nomor_tlp

            SqlCommand cmd = new SqlCommand("SELECT p.id_proyek AS id_proyek, p.nama_proyek AS nama_proyek, u.nomor_tlp AS nomor_tlp " +
                                    "FROM proyek AS p " +
                                    "JOIN kelompok AS k ON p.id_kel = k.id_kel " +
                                    "JOIN detail_kelompok AS dk ON k.id_kel = dk.id_kel " +
                                    "JOIN [user] AS u ON dk.id_user = u.id_user " +
                                    "WHERE p.id_proyek = @id_proyek", con);
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

            SqlCommand cmd = new SqlCommand("SELECT p.id_proyek AS id_proyek, p.nama_proyek AS nama_proyek, u.nomor_tlp AS nomor_tlp, k.nama_kel AS nama_kel " +
                                    "FROM proyek AS p " +
                                    "JOIN kelompok AS k ON p.id_kel = k.id_kel " +
                                    "JOIN detail_kelompok AS dk ON k.id_kel = dk.id_kel " +
                                    "JOIN [user] AS u ON dk.id_user = u.id_user " +
                                    "WHERE p.id_proyek = @id_proyek", con);
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

        public List<ProyekModel> getAllDataNotification(UserModel user, string dateNow) // ini buat ngambil semua data proyek yang berkaitan dengan user yang login
        {
            List<ProyekModel> proyek = new List<ProyekModel>();
            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("Select p.id_proyek as id_proyek ,p.nama_proyek as nama_proyek, k.nama_kel as nama_kel from proyek as p join kelompok as k on p.id_kel = k.id_kel WHERE p.status != 0 AND p.pic LIKE '" + namaUser + "' AND '"+ dateNow +"' >= p.target AND p.progress < 100", con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                proyek.Add(new ProyekModel()
                {
                    id_proyek = Convert.ToInt32(dr["id_proyek"].ToString()),
                    nama_proyek = dr["nama_proyek"].ToString(),
                    namakel = dr["nama_kel"].ToString(),
                });
            };
            dr.Close();
            con.Close();
            return proyek;
        }

    }
}