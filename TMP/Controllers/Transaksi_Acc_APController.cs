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
    public class Transaksi_Acc_APController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Transaksi_Acc_AP Transaksi_Acc_AP = new Transaksi_Acc_AP();
        Proyek _proyek = new Proyek();

        // GET: Transaksi_Acc_AP
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(Transaksi_Acc_AP.getAllData(user));
        }

        public ActionResult ACC_AP(int id, ProyekModel proyekModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spACCAP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", id);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // Dapatkan nilai kelompok yang dipilih dari model
            int selectedProyekId = id;

            string[] recipientNumbers = _proyek.getDataContacts(selectedProyekId); // Assume getDataContacts returns an array of recipient numbers
            string namaProyek = _proyek.getDataNamaProyek(selectedProyekId);
            string namaKel = _proyek.getDataNamaKelompok(selectedProyekId);
            string message = "📢 *PEMBERITAHUAN* 📢\n\n";
            message += "Kami telah menerima pengajuan rancangan proyek terkait *" + namaProyek + "* dari kelompok *" + namaKel + "*.\n";
            message += "Mohon segera melakukan progress berdasarkan pengajuan rancangan kegiatan yang Anda buat.\n";
            message += "Terima kasih atas partisipasi Anda.";

            // Combine recipient numbers into a single string separated by commas
            string recipients = string.Join(",", recipientNumbers);

            string url = $"https://api.whatsapp.com/send?phone={recipients}&text={Uri.EscapeDataString(message)}";

            TempData["message"] = "Activity Plan telah disetujui";

            // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
            string script = $"<script>window.open('{url}', '_blank');</script>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
            };

            /*TempData["message"] = "Activity Plan telah disetujui";
            return RedirectToAction("Index");*/
        }

        public ActionResult Tolak_AP(int id, ProyekModel proyekModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spdeleteAP", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@p1", id);
            //cmd.Parameters.AddWithValue("@status", 1);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            // Dapatkan nilai kelompok yang dipilih dari model
            int selectedProyekId = id;

            string[] recipientNumbers = _proyek.getDataContacts(selectedProyekId); // Assume getDataContacts returns an array of recipient numbers
            string namaProyek = _proyek.getDataNamaProyek(selectedProyekId);
            string namaKel = _proyek.getDataNamaKelompok(selectedProyekId);
            string message = "📢 *PEMBERITAHUAN* 📢\n\n";
            message += "Terima kasih atas pengajuan rancangan proyek terkait *" + namaProyek + "* dari kelompok *" + namaKel + "*.\n";
            message += "Namun, sayangnya pengajuan ini belum dapat kami setujui saat ini.\n";
            message += "Kami mengharapkan agar Anda segera merevisi rancangan kegiatan ini berdasarkan masukan dari tim kami.\n";
            message += "Kirimkan kembali rancangan yang telah direvisi secepatnya agar dapat kami proses lebih lanjut.\n";
            message += "Terima kasih atas perhatian dan kerjasamanya. Kami menantikan pengajuan yang diperbarui dari Anda segera.";

            // Combine recipient numbers into a single string separated by commas
            string recipients = string.Join(",", recipientNumbers);

            string url = $"https://api.whatsapp.com/send?phone={recipients}&text={Uri.EscapeDataString(message)}";

            TempData["ErrorMessage"] = "Activity Plan ditolak";

            // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
            string script = $"<script>window.open('{url}', '_blank');</script>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
            };

            /*TempData["ErrorMessage"] = "Activity Plan ditolak";
            return RedirectToAction("Index");*/
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            return View(Transaksi_Acc_AP.getData(id));
        }
    }
}