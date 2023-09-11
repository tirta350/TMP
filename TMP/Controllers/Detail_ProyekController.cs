using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TMP.Controllers
{
    public class Detail_ProyekController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Detail_Proyek detail_proyek = new Detail_Proyek();
        static List<cart> li = new List<cart>();

        // GET: Detail_Proyek
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(detail_proyek.getAllData(user));
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                if (TempData["cart"] == null)
                {
                    Session["cart_jumlah"] = 0;
                }
                else
                {
                    string ada = "";
                    foreach (var item in li)
                    {

                    }
                    Session["C"] = ada;
                }
                TempData.Keep();

                Detail_ProyekModel detail_Proyek = new Detail_ProyekModel();
                return View(detail_Proyek);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Detail_ProyekModel model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();

            c.nama_kegiatan = model.nama_kegiatan;

            if (TempData["cart"] == null)
            {
                li.Add(c);
                TempData["cart"] = li;
            }
            else
            {
                //List<cart> li2 = TempData["cart"] as List<cart>;
                string ada = "";
                foreach (var item in li)
                {
                    if (item.nama_kegiatan == c.nama_kegiatan)
                    {
                        TempData["ErrorMessage"] = "Data Telah Ditambahkan";
                        TempData.Keep();
                        return RedirectToAction("Create");
                    }
                }

                if (ada == "")
                {
                    TempData["SuccessMessage"] = "Data Ditemukan";
                    li.Add(c);
                }
                TempData["cart"] = li;
            }
            //}

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Detail_ProyekModel model, int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            if (TempData["cart"] != null)
            {
                //List<cart> li = TempData["cart"] as List<cart>;

                SqlCommand cmd = new SqlCommand("spproyekupdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_proyek", model.id_proyek);
                cmd.Parameters.AddWithValue("@progress", 0);
                cmd.Parameters.AddWithValue("@status", 2);


                con.Open();
                cmd.ExecuteScalar();
                con.Close();

                // Cari dulu id dari kelompok yang terakhir diinsert
                // baru di looping insert detail

                foreach (var item in li)
                {
                    SqlCommand cmd2 = new SqlCommand("spdetailinsert", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@id_proyek", model.id_proyek);
                    cmd2.Parameters.AddWithValue("@nama_kegiatan", item.nama_kegiatan);
                    cmd2.Parameters.AddWithValue("@status", 1);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                li.Clear();
                // Dapatkan nilai kelompok yang dipilih dari model
                int selectedProyekId = model.id_proyek;
                UserModel user = (UserModel)Session["user"];
                Detail_Proyek detail_Proyek = new Detail_Proyek();

                string recipientNumber = detail_proyek.getDataContact(selectedProyekId);
                string namaProyek = detail_proyek.getDataNamaProyek(selectedProyekId);
                string namaKel = detail_proyek.getDataNamaKelompok(selectedProyekId);
                string message = "📢 *PEMBERITAHUAN* 📢\n\n";
                message += "Pengajuan rancangan kegiatan sudah kami lakukan terkait dengan *" + namaProyek + "* kelompok *" + namaKel + "*.\n";
                message += "Harap segera melakukan konfirmasi dan memberikan tanggapan terkait pengajuan ini.\n";
                message += "Terima kasih.";

                string url = $"https://api.whatsapp.com/send?phone={recipientNumber}&text={Uri.EscapeDataString(message)}";

                TempData["message"] = "Rancangan Kegiatan Proyek Berhasil di Tambahkan";

                // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
                string script = $"<script>window.open('{url}', '_blank');</script>";

                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
                };
            }
            else
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return RedirectToAction("Create");

            }
        }

        public ActionResult Clear_Cart(string id)
        {
            var items = TempData["cart"] as List<cart>; // mengambil list dari session

            if (items != null)
            {
                var item = items.FirstOrDefault(i => i.nama_kegiatan == id); // mencari item di dalam list

                if (item != null)
                {
                    items.Remove(item); // menghapus item dari list
                }
            }

            return RedirectToAction("Create"); // redirect kembali ke halaman index
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                return View(detail_proyek.getData(id));
            }
        }

        public ActionResult Delete(int id, Detail_ProyekModel detail_ProyekModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spdeleteAP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", id);
            //cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("Index");
        }
    }
}