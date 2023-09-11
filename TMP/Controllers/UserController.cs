using TMP.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TMP.Controllers
{
    public class UserController : Controller
    {
        UserModel userModel = new UserModel();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        User _user = new User();

        // GET: User
        public ActionResult Index()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }

            return View(_user.getAllData()); ;
        }

        public ActionResult Create()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }

            UserModel userModel = new UserModel();
            return View(userModel);
        }

        //insert
        [HttpPost]
        public ActionResult Create(UserModel userModel) // ini buat insert data user
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }

            try
            {
                SqlCommand cmd = new SqlCommand("spuserinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nama_user", userModel.nama_user);
                cmd.Parameters.AddWithValue("@username", userModel.username);
                cmd.Parameters.AddWithValue("@password", userModel.password);
                cmd.Parameters.AddWithValue("@email", userModel.email);
                cmd.Parameters.AddWithValue("@nomor_tlp", userModel.nomor_tlp);
                cmd.Parameters.AddWithValue("@alamat", userModel.alamat);
                cmd.Parameters.AddWithValue("@jenis_kelamin", userModel.jenis_kelamin);
                cmd.Parameters.AddWithValue("@role", userModel.role);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["message"] = "Data User Berhasil di Tambahkan";
                return RedirectToAction("Index");
            }

            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return View(userModel);
            }
        }

        // GET: Edit
        public ActionResult Update(int id)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel userModel = _user.getData(id);
            return View(userModel);
        }

        [HttpPost]
        public ActionResult Update(int id, UserModel userModel)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            try
            {
                SqlCommand cmd = new SqlCommand("spuserupdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_user", id);
                cmd.Parameters.AddWithValue("@nama_user", userModel.nama_user);
                cmd.Parameters.AddWithValue("@username", userModel.username);
                cmd.Parameters.AddWithValue("@password", userModel.password);
                cmd.Parameters.AddWithValue("@email", userModel.email);
                cmd.Parameters.AddWithValue("@nomor_tlp", userModel.nomor_tlp);
                cmd.Parameters.AddWithValue("@alamat", userModel.alamat);
                cmd.Parameters.AddWithValue("@jenis_kelamin", userModel.jenis_kelamin);
                cmd.Parameters.AddWithValue("@role", userModel.role);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["message"] = "Data User Berhasil di Ubah";
                return RedirectToAction("Index");
            }
            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return View(userModel);
            }
        }

        public ActionResult Delete(int id, UserModel userModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spdeleteuser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel userModel = _user.getData(id);
            return View(userModel);
        }
    }
}