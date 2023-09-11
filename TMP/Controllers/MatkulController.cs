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
    public class MatkulController : Controller
    {
        MatkulModel matkulModel = new MatkulModel();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Matkul _matkul = new Matkul();
        Prodi _prodi = new Prodi();

        // GET: Section
        public ActionResult Index()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            return View(_matkul.getAllData());
        }
        public ActionResult Create()
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            MatkulModel matkulModel = new MatkulModel();
            return View(matkulModel);
        }

        //insert
        [HttpPost]
        public ActionResult Create(MatkulModel matkulModel) // ini buat insert data user
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            try
            {

                SqlCommand cmd = new SqlCommand("spmatkulinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_prodi", matkulModel.id_prodi);
                cmd.Parameters.AddWithValue("@nama_matkul", matkulModel.nama_matkul);
                cmd.Parameters.AddWithValue("@dosen_pengampu", matkulModel.dosen_pengampu);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["message"] = "Data Mata Kuliah Berhasil di Tambahkan";
                return RedirectToAction("Index");
            }

            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return View(matkulModel);
            }


        }

        // GET: Edit
        public ActionResult Update(int id)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            MatkulModel matkulModel = _matkul.getData(id);
            return View(matkulModel);
        }

        [HttpPost]
        public ActionResult Update(int id, MatkulModel matkulModel)
        {
            // Throw session timeout
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            try
            {
                SqlCommand cmd = new SqlCommand("spmatkulupdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_matkul", id);
                cmd.Parameters.AddWithValue("@id_prodi", matkulModel.id_prodi);
                cmd.Parameters.AddWithValue("@nama_matkul", matkulModel.nama_matkul);
                cmd.Parameters.AddWithValue("@dosen_pengampu", matkulModel.dosen_pengampu);
                cmd.Parameters.AddWithValue("@status", 1);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                TempData["message"] = "Data Mata Kuliah Berhasil di Ubah";
                return RedirectToAction("Index");

            }
            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return View(matkulModel);
            }
            
        }

        public ActionResult Delete(int id, MatkulModel matkulModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spmatkuldelete", con);
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
                return RedirectToAction("User", "Login");
            }

            MatkulModel matkulModel = _matkul.getData(id);
            return View(matkulModel);
        }
    }
}