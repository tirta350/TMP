using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TMP.Models
{
    public class User
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        UserKelompokModel listUserKelompok;
        Kelompok kelompok = new Kelompok();
        // Login Needs
        public UserKelompokModel getall()
        {

            List<UserModel> userModel = getAllDataMahasiswa();
            List<KelompokModel> kelompokModels = kelompok.getAllData();

            UserKelompokModel userKelompokModel = new UserKelompokModel();
            userKelompokModel.listuser = new List<UserModel>();

            userKelompokModel.listuser = getAllDataMahasiswa();


            return userKelompokModel;
        }
        // Login Needs
        public UserModel getUser(string username, string password) // ini buat ngambil data usernya
        {
            UserModel user = new UserModel();
            SqlCommand cmd = new SqlCommand("Select * from [user] where username like @username and password like @password", con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                user.id_user = Convert.ToInt16(dr["id_user"].ToString());
                user.nama_user = dr["nama_user"].ToString();
                user.username = dr["username"].ToString();
                user.password = dr["password"].ToString();
                user.email = dr["email"].ToString();
                user.nomor_tlp = dr["nomor_tlp"].ToString();
                user.alamat = dr["alamat"].ToString();
                user.jenis_kelamin = dr["jenis_kelamin"].ToString();
                user.role = dr["role"].ToString();
                user.status = Convert.ToInt16(dr["status"].ToString());
                dr.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                dr.Close();
                con.Close();
            }
            return user;
        }

        public Boolean isUserValid(string username, string password) // ini buat ngecheck data user yang login valid apa enggak
        {
            SqlCommand cmd = new SqlCommand("Select * from [User] where username like @username and password like @password and status = @status", con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();

            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) { dr.Close(); con.Close(); return true; } else { dr.Close(); con.Close(); return false; }
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        internal User getData(object id)
        {
            throw new NotImplementedException();
        }


        // CRUD Needs

        public Boolean isUniqueUSERNAME(string username) // ini buat ngecheck NIKnya udah unik apa belom , kalau tidak ditemukan return false dan sebaliknya
        {
            SqlCommand cmd = new SqlCommand("Select * from [User] where username like @username", con);
            cmd.Parameters.AddWithValue("@username", username);
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

        public Boolean isData(string username) // ini buat ngecheck datanya aktif atau belom kena delete
        {
            SqlCommand cmd = new SqlCommand("Select * from [User] where username like @username and status = @status", con);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            try
            {
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows) { dr.Close(); con.Close(); return true; } else { dr.Close(); con.Close(); return false; }
            }
            catch (Exception ex)
            {
                con.Close();
                return false;
            }
        }

        public List<UserModel> getAllData() // ini buat ngambil semua data user
        {
            List<UserModel> users = new List<UserModel>();
            SqlCommand cmd = new SqlCommand("Select * from [User] where status = @status", con);
            cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                users.Add(new UserModel()
                {
                    id_user = Convert.ToInt32(dr["id_user"].ToString()),
                    nama_user = dr["nama_user"].ToString(),
                    username = dr["username"].ToString(),
                    password = dr["password"].ToString(),
                    email = dr["email"].ToString(),
                    nomor_tlp = dr["nomor_tlp"].ToString(),
                    alamat = dr["alamat"].ToString(),
                    jenis_kelamin = dr["jenis_kelamin"].ToString(),
                    role = dr["role"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return users;
        }

        public List<UserModel> getAllDataMahasiswa() // ini buat ngambil semua data user
        {
            List<UserModel> users = new List<UserModel>();
            SqlCommand cmd = new SqlCommand("Select * from [User] where status = @status and role = @role", con);
            cmd.Parameters.AddWithValue("@status", 1);
            cmd.Parameters.AddWithValue("@role", "Mahasiswa");
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                users.Add(new UserModel()
                {
                    id_user = Convert.ToInt32(dr["id_user"].ToString()),
                    nama_user = dr["nama_user"].ToString(),
                    username = dr["username"].ToString(),
                    password = dr["password"].ToString(),
                    email = dr["email"].ToString(),
                    nomor_tlp = dr["nomor_tlp"].ToString(),
                    alamat = dr["alamat"].ToString(),
                    jenis_kelamin = dr["jenis_kelamin"].ToString(),
                    role = dr["role"].ToString(),
                    status = Convert.ToInt32(dr["status"].ToString()),
                });
            };
            dr.Close();
            con.Close();
            return users;
        }

        public UserModel getData(int id)
        {
            UserModel user = new UserModel();
            SqlCommand cmd = new SqlCommand("Select * from [User] where id_user like @id", con);
            cmd.Parameters.AddWithValue("@id", id);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();

            if (dr.HasRows)
            {
                user.id_user = Convert.ToInt32(dr["id_user"].ToString());
                user.nama_user = dr["nama_user"].ToString();
                user.username = dr["username"].ToString();
                user.password = dr["password"].ToString();
                user.email = dr["email"].ToString();
                user.nomor_tlp = dr["nomor_tlp"].ToString();
                user.alamat = dr["alamat"].ToString();
                user.jenis_kelamin = dr["jenis_kelamin"].ToString();
                user.role = dr["role"].ToString();
                user.status = Convert.ToInt32(dr["status"].ToString());
            }
            else
            {

            }
            dr.Close();
            con.Close();
            return user;
        }

        //insert
        //public Boolean insert(UserModel userModel) // ini buat insert data user
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("spuserinsert", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@nama_user", userModel.nama_user);
        //        cmd.Parameters.AddWithValue("@username", userModel.username);
        //        cmd.Parameters.AddWithValue("@password", userModel.password);
        //        cmd.Parameters.AddWithValue("@email", userModel.email);
        //        cmd.Parameters.AddWithValue("@nomor_tlp", userModel.nomor_tlp);
        //        cmd.Parameters.AddWithValue("@alamat", userModel.alamat);
        //        cmd.Parameters.AddWithValue("@jenis_kelamin", userModel.jenis_kelamin);
        //        cmd.Parameters.AddWithValue("@role", userModel.role);
        //        cmd.Parameters.AddWithValue("@status", 1);
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //        return false;
        //    }
        //}

        ////update
        //public Boolean update(UserModel userModel) // ini buat insert data user
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("spuserupdate", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@id_user", userModel.id_user);
        //        cmd.Parameters.AddWithValue("@nama_user", userModel.nama_user);
        //        cmd.Parameters.AddWithValue("@username", userModel.username);
        //        cmd.Parameters.AddWithValue("@password", userModel.password);
        //        cmd.Parameters.AddWithValue("@email", userModel.email);
        //        cmd.Parameters.AddWithValue("@nomor_tlp", userModel.nomor_tlp);
        //        cmd.Parameters.AddWithValue("@alamat", userModel.alamat);
        //        cmd.Parameters.AddWithValue("@jenis_kelamin", userModel.jenis_kelamin);
        //        cmd.Parameters.AddWithValue("@role", userModel.role);
        //        cmd.Parameters.AddWithValue("@status", 1);

        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //        return false;
        //    }
        //}

        //public Boolean delete(string id)
        //{
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand("spuserdelete", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@id_user", id);
        //        cmd.Parameters.AddWithValue("@status", 0);
        //        con.Open();
        //        cmd.ExecuteNonQuery();
        //        con.Close();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        con.Close();
        //        return false;
        //    }
        //}


    }
}