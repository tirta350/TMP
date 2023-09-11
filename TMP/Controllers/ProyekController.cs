using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace TMP.Controllers
{
    public class ProyekController : Controller
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        ProyekModel proyekModel = new ProyekModel();
        Proyek _proyek = new Proyek();
        private Proyek pr = new Proyek();
        private Matkul matkul = new Matkul();
        List<cart> li = new List<cart>();

        // GET: Section
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(_proyek.getAllData(user));
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            if (TempData["cart"] == null)
            {
                Session["cart_jumlah"] = 0;
            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int ada = 0;
                foreach (var item in li2)
                {

                }
                Session["C"] = ada;
            }
            TempData.Keep();
            UserKelompokModel matkulmodel = matkul.getall();
            matkulmodel.proyek = new ProyekModel();
            return PartialView(matkulmodel);
        }


        // POST: kelompoks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //insert
        [HttpPost]
        public ActionResult Create([Bind(Include = "id")] model model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();
            MatkulModel detil = matkul.getData(model.id);
            if (detil == null)
            {
                TempData["ErrorMessage"] = "Data Tidak Ditemukan";
                return RedirectToAction("Create");
            }
            else
            {

                c.id = detil.id_matkul;
                c.nama = detil.nama_matkul;

                if (TempData["cart"] == null)
                {
                    li.Add(c);
                    TempData["cart"] = li;
                }
                else
                {
                    List<cart> li2 = TempData["cart"] as List<cart>;
                    int ada = 0;
                    foreach (var item in li2)
                    {
                        if (item.id == c.id)
                        {
                            TempData["SuccessMessage"] = "Data Telah Ditambahkan";
                            TempData.Keep();
                            return RedirectToAction("Create");
                        }
                    }

                    if (ada == 0)
                    {
                        TempData["SuccessMessage"] = "Data Ditemukan";
                        li2.Add(c);
                    }
                    TempData["cart"] = li2;
                }
            }

            return RedirectToAction("Create");
        }

        public ActionResult Clear_Cart(int id)
        {
            var items = TempData["cart"] as List<cart>;
            var item = items.FirstOrDefault(i => i.id == id); // mencari item di dalam list

            if (item != null)
            {
                items.Remove(item); // menghapus item dari list
            }

            return RedirectToAction("Create");
        }

        public ActionResult Save()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(UserKelompokModel userKelompokModel, int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            if (TempData["cart"] != null)
            {

                List<cart> li = TempData["cart"] as List<cart>;
                UserModel user = (UserModel)Session["user"];
                int primaryKey;

                SqlCommand cmd = new SqlCommand("spproyekinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_kel", userKelompokModel.proyek.id_kel);
                cmd.Parameters.AddWithValue("@nama_proyek", userKelompokModel.proyek.nama_proyek);
                cmd.Parameters.AddWithValue("@target", userKelompokModel.proyek.target);
                cmd.Parameters.AddWithValue("@tanggal_mulai", userKelompokModel.proyek.tanggal_mulai);
                cmd.Parameters.AddWithValue("@semester", userKelompokModel.proyek.semester);
                cmd.Parameters.AddWithValue("@pic", @user.nama_user);
                cmd.Parameters.AddWithValue("@progress", 0);
                cmd.Parameters.AddWithValue("@status", 1);

                con.Open();
                primaryKey = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                // Cari dulu id dari kelompok yang terakhir diinsert
                // baru di looping insert detail

                foreach (var item in li)
                {
                    MatkulModel detil = matkul.getData(item.id);
                    SqlCommand cmd2 = new SqlCommand("spdetailmatkulinsert", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@id_matkul", detil.id_matkul);
                    cmd2.Parameters.AddWithValue("@id_proyek", primaryKey);
                    cmd2.Parameters.AddWithValue("@nama_matkul", detil.nama_matkul);
                    cmd2.Parameters.AddWithValue("@status", 1);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                TempData.Remove("cart");

                // Dapatkan nilai kelompok yang dipilih dari model
                int selectedProyekId = primaryKey;

                string[] recipientNumbers = _proyek.getDataContacts(selectedProyekId); // Assume getDataContacts returns an array of recipient numbers
                string namaProyek = _proyek.getDataNamaProyek(selectedProyekId);
                string namaKel = _proyek.getDataNamaKelompok(selectedProyekId);
                string message = "📢 *PEMBERITAHUAN* 📢\n\n";
                message += "Proyek telah kami bagikan terkait dengan *" + namaProyek + "* kelompok *" + namaKel + "*.\n";
                message += "Mohon segera melakukan konfirmasi dan memberikan tanggapan terkait pengajuan rancangan kegiatan.\n";
                message += "Terima kasih.";

                // Combine recipient numbers into a single string separated by commas
                string recipients = string.Join(",", recipientNumbers);

                string url = $"https://api.whatsapp.com/send?phone={recipients}&text={Uri.EscapeDataString(message)}";

                TempData["message"] = "Data Proyek Berhasil di Tambahkan";

                // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
                string script = $"<script>window.open('{url}', '_blank');</script>";

                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
                };


                /*TempData["message"] = "Data Proyek Berhasil di Tambahkan";
                return RedirectToAction("Index");*/
            }
            else
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return RedirectToAction("Create");

            }
        }

        public ActionResult Delete(int id, ProyekModel proyekModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spproyekdelete", con);
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
            if (Session["user"] == null)
            {

                return RedirectToAction("User", "Login");
            }
            //ProyekModel proyekModel = _proyek.getData(id);
            return View(_proyek.getData(id));
        }

        public PartialViewResult nama_matkul()
        {
            MatkulModel matkulModel = new MatkulModel();
            return PartialView(matkulModel);
        }
    }
}