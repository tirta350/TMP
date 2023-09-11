using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class Prodi
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<ProdiModel> getAllData() // ini buat ngambil semua data prodi
        {
            List<ProdiModel> prodi = new List<ProdiModel>();

            SqlCommand cmd = new SqlCommand("Select * from Prodi where status = @status", con);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                prodi.Add(new ProdiModel()
                {
                    id_prodi = Convert.ToInt32(dr["id_prodi"].ToString()),
                    nama_prodi = dr["nama_prodi"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString())
                });
            };
            dr.Close();
            con.Close();
            return prodi;
        }

        public ProdiModel getData(int id)
        {
            ProdiModel prodi = new ProdiModel();
            SqlCommand cmd = new SqlCommand("Select * from Prodi where id_prodi = @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {

                prodi.id_prodi = Convert.ToInt32(dr[0].ToString());
                prodi.nama_prodi = dr[1].ToString();
                prodi.status = Convert.ToInt32(dr[2].ToString());
            }
            else
            {

            }

            dr.Close();
            con.Close();
            return prodi;
        }

        //insert
        public Boolean insert(ProdiModel prodiModel) // ini buat insert data user
        {
            try
            {
                SqlCommand cmd = new SqlCommand("spprodiinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nama_prodi", prodiModel.nama_prodi);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

    }
}