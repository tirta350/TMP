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
    public class Transaksi_Acc_StepController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Transaksi_Acc_Step Transaksi_Acc_Step = new Transaksi_Acc_Step();
        Proyek _proyek = new Proyek();

        // GET: Transaksi_Acc_Step
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(Transaksi_Acc_Step.getAllData(user));
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            return View(Transaksi_Acc_Step.getData(id));
        }

        public ActionResult Lihat_Progress(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            Detail_ProyekModel detail_Proyek = Transaksi_Acc_Step.getDataProgress(id);
            return View(detail_Proyek);
        }

        //insert
        [HttpPost, ValidateInput(false)]
        public ActionResult Lihat_Progress(Detail_ProyekModel detail_Proyek, string komentar) // ini buat upload progress
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            try
            {
                SqlCommand cmd = new SqlCommand("spKomentar", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id_detail", detail_Proyek.id_detail);
                cmd.Parameters.AddWithValue("@komentar", komentar);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                int selectedProyekId = Convert.ToInt32(detail_Proyek.id_proyek);
                int? selectedDetailId = Convert.ToInt32(detail_Proyek.id_detail);
                UserModel user = (UserModel)Session["user"];

                string[] recipientNumbers = _proyek.getDataContacts(selectedProyekId); // Assume getDataContacts returns an array of recipient numbers
                string namaProyek = _proyek.getDataNamaProyek(selectedProyekId);
                string namaKel = _proyek.getDataNamaKelompok(selectedProyekId);
                string namaKegiatan = _proyek.getDataNamaKegiatan(selectedDetailId);
                string message = "📢 *PEMBERITAHUAN* 📢\n\n";
                message += "Adanya perubahan informasi terkait komentar pada progress kegiatan *" + namaKegiatan + "* terkait dengan *" + namaProyek + "* kelompok *" + namaKel + "*.\n";
                message += "Mohon segera melakukan konfirmasi dan memberikan tanggapan terkait perubahan ini.\n";
                message += "Terima kasih atas partisipasinya.";

                // Combine recipient numbers into a single string separated by commas
                string recipients = string.Join(",", recipientNumbers);

                string url = $"https://api.whatsapp.com/send?phone={recipients}&text={Uri.EscapeDataString(message)}";

                TempData["message"] = "Data Komentar Berhasil diubah";

                // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
                string script = $"<script>window.open('{url}', '_blank');</script>";

                return new ContentResult
                {
                    ContentType = "text/html",
                    Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
                };

                /*TempData["message"] = "Data Komentar Berhasil diubah";
                return RedirectToAction("Index");*/
            }
            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Data Komentar!";
                return View(detail_Proyek);
            }

        }

        public ActionResult DownloadFile(string tahap)
        {
            string filename = tahap;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Upload_File\\" + filename;
            byte[] filedata = System.IO.File.ReadAllBytes(filepath);
            string contentType = MimeMapping.GetMimeMapping(filepath);

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(filedata, contentType);
        }

        public ActionResult ACC_Step(string id_proyek, string id_detail, string progress)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spACC_Step", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_detail", id_detail);
            cmd.Parameters.AddWithValue("@id_proyek", id_proyek);
            cmd.Parameters.AddWithValue("@progress", progress);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

            int selectedProyekId = Convert.ToInt32(id_proyek);
            int? selectedDetailId = Convert.ToInt32(id_detail);
            UserModel user = (UserModel)Session["user"];

            string[] recipientNumbers = _proyek.getDataContacts(selectedProyekId); // Assume getDataContacts returns an array of recipient numbers
            string namaProyek = _proyek.getDataNamaProyek(selectedProyekId);
            string namaKel = _proyek.getDataNamaKelompok(selectedProyekId);
            string namaKegiatan = _proyek.getDataNamaKegiatan(selectedDetailId);
            string message = "📢 *PEMBERITAHUAN* 📢\n\n";
            message += "Progress kegiatan *" + namaKegiatan + "* telah kami terima terkait dengan *" + namaProyek + "* kelompok *" + namaKel + "*.\n";
            message += "Terima kasih atas partisipasinya.";

            // Combine recipient numbers into a single string separated by commas
            string recipients = string.Join(",", recipientNumbers);

            string url = $"https://api.whatsapp.com/send?phone={recipients}&text={Uri.EscapeDataString(message)}";

            TempData["message"] = "Progress Proyek telah disetujui";

            // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
            string script = $"<script>window.open('{url}', '_blank');</script>";

            return new ContentResult
            {
                ContentType = "text/html",
                Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
            };

            /*TempData["message"] = "Progress Proyek telah disetujui";
            return RedirectToAction("Index");*/
        }
    }
}