using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Models
{
    public class Kelompok
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public List<KelompokModel> getAllData() // ini buat ngambil semua data kelompok
        {
            List<KelompokModel> kelompok = new List<KelompokModel>();

            SqlCommand cmd = new SqlCommand("Select * from Kelompok where status = @status", con);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                kelompok.Add(new KelompokModel()
                {
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_kel = dr["nama_kel"].ToString(),
                    tahun = dr["tahun"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString())
                });
            };
            dr.Close();
            con.Close();
            return kelompok;
        }

        public List<Detail_KelompokModel> getAllDataDetail(int id) // ini buat ngambil semua data kelompok
        {
            List<Detail_KelompokModel> detail_kel = new List<Detail_KelompokModel>();

            SqlCommand cmd = new SqlCommand("Select * from detail_kelompok where id_kel = @id_kel AND status = '1'", con);
            cmd.Parameters.AddWithValue("@id_kel", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                detail_kel.Add(new Detail_KelompokModel()
                {
                    id_detailkel = Convert.ToInt32(dr["id_detailkel"].ToString()),
                    id_user = dr["id_user"].ToString(),
                    id_kel = Convert.ToInt32(dr["id_kel"].ToString()),
                    nama_anggota = dr["nama_anggota"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString())
                });
            };
            dr.Close();
            con.Close();
            return detail_kel;
        }

        public KelompokModel getData(int id_kel)
        {
            KelompokModel kelompok = new KelompokModel();
            //Detail_KelompokModel detail_kel = new Detail_KelompokModel();
            SqlCommand cmd = new SqlCommand("Select * from kelompok where id_kel = @id_kel and status = @status", con);
            cmd.Parameters.AddWithValue("@id_kel", id_kel);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {

                kelompok.id_kel = Convert.ToInt32(dr[0].ToString());
                kelompok.nama_kel = dr[1].ToString();
                kelompok.tahun = dr[2].ToString();
                kelompok.status = Convert.ToInt32(dr[3].ToString());
                //detail_kel.id_detailkel = Convert.ToInt32(dr[3].ToString());
                //detail_kel.id_kel = Convert.ToInt32(dr[4].ToString());
                //detail_kel.id_user = Convert.ToInt32(dr[5].ToString());
                //detail_kel.nama_anggota = dr[6].ToString();

            }
            else
            {

            }

            dr.Close();
            con.Close();
            return kelompok;
        }
    }
}