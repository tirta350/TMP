using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Matkul
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        Prodi _prodi = new Prodi();

        //Proyek proyek = new Proyek();

        //UserkelompokModel list;

        public UserKelompokModel getall()
        {

            //List<MatkulModel> matkulModel = getAllData();
            //List<ProyekModel> proyekModel = proyek.getAllData();

            UserKelompokModel userKelompokModel = new UserKelompokModel();
            userKelompokModel.listmatkul = new List<MatkulModel>();

            userKelompokModel.listmatkul = getAllData();


            return userKelompokModel;
        }

        public List<MatkulModel> getAllData() // ini buat ngambil semua data matkul
        {
            List<MatkulModel> matkul = new List<MatkulModel>();

            SqlCommand cmd = new SqlCommand("Select * from mata_kuliah where status = @status", con);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                ProdiModel prodi = _prodi.getData(Convert.ToInt32(dr["id_prodi"]));

                matkul.Add(new MatkulModel()
                {
                    id_matkul = Convert.ToInt32(dr["id_matkul"].ToString()),
                    id_prodi = Convert.ToInt32(dr["id_prodi"].ToString()),
                    prodinama = prodi.nama_prodi, // ini data prodinya yang udah diambil
                    nama_matkul = dr["nama_matkul"].ToString(),
                    dosen_pengampu = dr["dosen_pengampu"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return matkul;
        }

        public Boolean isUniqueID(string id) // ini buat ngecheck no_asset nya udah unik apa belom , kalau tidak ditemukan return false dan sebaliknya
        {
            SqlCommand cmd = new SqlCommand("Select * from mata_kuliah where id_matkul like @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) { dr.Close(); con.Close(); return false; } else { dr.Close(); con.Close(); return true; }
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        public MatkulModel getData(int id)
        {
            MatkulModel matkul = new MatkulModel();
            SqlCommand cmd = new SqlCommand("Select * from mata_kuliah where id_matkul like @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            ProdiModel prodi = _prodi.getData(Convert.ToInt32(dr["id_prodi"]));

            matkul.id_matkul = Convert.ToInt32(dr["id_matkul"]);
            matkul.nama_matkul = dr["nama_matkul"].ToString();
            matkul.dosen_pengampu = dr["dosen_pengampu"].ToString();
            matkul.id_prodi = Convert.ToInt32(dr["id_prodi"]);
            matkul.prodinama = prodi.nama_prodi; // ini data prodinya yang udah diambil
            matkul.status = Convert.ToInt32(dr["status"]);

            dr.Close();
            con.Close();
            return matkul;
        }
    }
}