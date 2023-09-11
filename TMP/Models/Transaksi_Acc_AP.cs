using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Transaksi_Acc_AP
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ProyekModel> getAllData(UserModel user) // ini buat ngambil semua data prodi
        {
            List<ProyekModel> proyek = new List<ProyekModel>();

            // Menggunakan nilai yang diteruskan dari sesi pengguna
            string namaUser = user.nama_user;

            SqlCommand cmd = new SqlCommand("SELECT * FROM proyek WHERE pic LIKE '" + namaUser + "' AND status='2'", con);
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

        //public Boolean isUniqueID(string id) // ini buat ngecheck no_asset nya udah unik apa belom , kalau tidak ditemukan return false dan sebaliknya
        //{
        //    SqlCommand cmd = new SqlCommand("Select * from proyek where id_proyek like @id", con);
        //    cmd.Parameters.AddWithValue("@id", id);
        //    con.Open();
        //    try
        //    {
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        if (dr.HasRows) { dr.Close(); con.Close(); return false; } else { dr.Close(); con.Close(); return true; }
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //        return false;
        //    }
        //}

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
    }
}