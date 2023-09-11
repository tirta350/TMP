using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;

namespace TMP.Controllers
{
    public class ProdiController : Controller
    {
        ProdiModel prodiModel = new ProdiModel();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Prodi _prodi = new Prodi();
        // GET: Section

        public ActionResult Index()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            return View(_prodi.getAllData());
        }

        //GET
        public ActionResult Create()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            ProdiModel ProdiModel = new ProdiModel();
            return View(ProdiModel);
        }

        //insert
        [HttpPost]
        public ActionResult Create(ProdiModel prodiModel) // ini buat insert data user
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            if (ModelState.IsValid)
            {
                if (_prodi.insert(prodiModel))
                {
                    TempData["message"] = "Data Program Studi Berhasil di Tambahkan";
                }
                else
                {
                    TempData["message"] = "Data Program Studi gagal di Tambahkan";
                }
            }
            else
            {
                Response.Write("<script>alert('Pesan: " + "Harap isi Semua Data!" + "');</script>");
                return View(prodiModel);
            }

            TempData["message"] = "Data Program Studi Berhasil di Tambahkan";
            return RedirectToAction("Index");

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

            ProdiModel prodiModel = _prodi.getData(id);
            return View(prodiModel);
        }

        [HttpPost]
        public ActionResult Update(int id, ProdiModel prodiModel)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }

            try
            {
                SqlCommand cmd = new SqlCommand("spprodiupdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_prodi", id);
                cmd.Parameters.AddWithValue("@nama_prodi", prodiModel.nama_prodi);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["message"] = "Data Program Studi Berhasil di Ubah";
                return RedirectToAction("Index");
            }
            catch
            {
                Response.Write("<script>alert('Pesan: " + "Harap Isi Semua Data!" + "');</script>");
                return View(prodiModel);
            }
            
        }

        public ActionResult Delete(int id, ProdiModel prodiModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spprodidelete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }
    }
}