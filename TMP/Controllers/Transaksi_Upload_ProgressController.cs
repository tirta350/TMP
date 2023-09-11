using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;

namespace TMP.Controllers
{
    public class Transaksi_Upload_ProgressController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Transaksi_Upload_Progress Transaksi_Upload_Progress = new Transaksi_Upload_Progress();

        // GET: Transaksi_Upload_Progress
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("OpsiLogin", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(Transaksi_Upload_Progress.getAllData(user));
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("OpsiLogin", "Login");
            }
            return View(Transaksi_Upload_Progress.getData(id));
        }

        public ActionResult Upload_Progress(int id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("OpsiLogin", "Login");
            }
            Detail_ProyekModel detail_Proyek = Transaksi_Upload_Progress.getDataProgress(id);
            return View(detail_Proyek);
        }

        //insert
        [HttpPost, ValidateInput(false)]
        public ActionResult Upload_Progress(Detail_ProyekModel detail_Proyek, int? id, string problem_identification, string corrective_action) // ini buat upload progress
        {
            try
            {
                if (Session["user"] == null)
                {
                    return RedirectToAction("User", "Login");
                }

                if (detail_Proyek.UploadFileOSP != null)
                {
                    string contentType = detail_Proyek.UploadFileOSP.ContentType;
                    // Periksa apakah file adalah word atau PDF
                    if (contentType != "application/pdf")
                    {
                        TempData["ErrorMessage"] = "File harus berbentuk word (.docx) atau PDF (.pdf).";
                        return RedirectToAction("Index");
                    }

                    string finalPath = "\\Upload_File\\" + detail_Proyek.UploadFileOSP.FileName;

                    detail_Proyek.UploadFileOSP.SaveAs(Server.MapPath("~") + finalPath);
                    detail_Proyek.pry_file = detail_Proyek.UploadFileOSP.FileName;

                    SqlCommand cmd = new SqlCommand("spUploadProgress", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_detail", detail_Proyek.id_detail);
                    cmd.Parameters.AddWithValue("@tahap", detail_Proyek.pry_file);
                    cmd.Parameters.AddWithValue("@problem_identification", detail_Proyek.problem_identification);
                    cmd.Parameters.AddWithValue("@corrective_action", detail_Proyek.corrective_action);
                    cmd.Parameters.AddWithValue("@status", 2);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    int selectedProyekId = detail_Proyek.id_proyek;
                    int? selectedDetailId = detail_Proyek.id_detail;
                    UserModel user = (UserModel)Session["user"];
                    Transaksi_Upload_Progress transaksi_Upload_Progress = new Transaksi_Upload_Progress();

                    string recipientNumber = transaksi_Upload_Progress.getDataContact(selectedProyekId);
                    string namaProyek = transaksi_Upload_Progress.getDataNamaProyek(selectedProyekId);
                    string namaKel = transaksi_Upload_Progress.getDataNamaKelompok(selectedProyekId);
                    string namaKegiatan = transaksi_Upload_Progress.getDataNamaKegiatan(selectedDetailId);
                    string message = "Progress kegiatan " + namaKegiatan + " sudah kami lakukan terkait dengan " + namaProyek + " kelompok " + namaKel;

                    string url = $"https://api.whatsapp.com/send?phone={recipientNumber}&text={Uri.EscapeDataString(message)}";

                    TempData["message"] = "Data Progress Berhasil di Tambahkan";

                    // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
                    string script = $"<script>window.open('{url}', '_blank');</script>";

                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
                    };

                    //TempData["message"] = "Data Progress Berhasil di Tambahkan";
                    //return RedirectToAction("Index");
                }
                else
                {
                    SqlCommand cmd = new SqlCommand("spUploadProgressnoFile", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_detail", detail_Proyek.id_detail);
                    cmd.Parameters.AddWithValue("@problem_identification", detail_Proyek.problem_identification);
                    cmd.Parameters.AddWithValue("@corrective_action", detail_Proyek.corrective_action);
                    cmd.Parameters.AddWithValue("@status", 2);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    int selectedProyekId = detail_Proyek.id_proyek;
                    int? selectedDetailId = detail_Proyek.id_detail;
                    UserModel user = (UserModel)Session["user"];
                    Transaksi_Upload_Progress transaksi_Upload_Progress = new Transaksi_Upload_Progress();

                    string recipientNumber = transaksi_Upload_Progress.getDataContact(selectedProyekId);
                    string namaProyek = transaksi_Upload_Progress.getDataNamaProyek(selectedProyekId);
                    string namaKel = transaksi_Upload_Progress.getDataNamaKelompok(selectedProyekId);
                    string namaKegiatan = transaksi_Upload_Progress.getDataNamaKegiatan(selectedDetailId);
                    string message = "📢 *PEMBERITAHUAN* 📢\n\n";
                    message += "Progress kegiatan *" + namaKegiatan + "* sudah kami lakukan terkait dengan *" + namaProyek + "* kelompok *" + namaKel + "*.\n";
                    message += "Mohon untuk segera memeriksa dan memberikan tanggapan terkait progress ini.\n";
                    message += "Terima kasih atas perhatiannya.";

                    string url = $"https://api.whatsapp.com/send?phone={recipientNumber}&text={Uri.EscapeDataString(message)}";

                    TempData["message"] = "Data Progress Berhasil di Tambahkan";

                    // Perintah JavaScript untuk membuka tautan di tab baru menggunakan window.open
                    string script = $"<script>window.open('{url}', '_blank');</script>";

                    return new ContentResult
                    {
                        ContentType = "text/html",
                        Content = $"{script}<script>window.location.href = '{Url.Action("Index")}';</script>"
                    };

                    //TempData["message"] = "Data Progress Berhasil di Tambahkan";
                    //return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return RedirectToAction("Index");
            }
        }

        public ActionResult DownloadFile(string tahap)
        {
            string filename = tahap;
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Upload_File\\" + filename;

            // Periksa ukuran file sebelum membaca dan mengirimkannya
            FileInfo fileInfo = new FileInfo(filepath);
            long fileSize = fileInfo.Length;
            long maxSize = 10 * 1024 * 1024; // 10 MB dalam byte

            if (fileSize > maxSize)
            {
                // Mengembalikan respons atau pesan yang sesuai jika ukuran file melebihi batas maksimum
                return Content("Ukuran file terlalu besar. Batas maksimum adalah 10 MB.");
            }

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

        public ActionResult GetFile(string tahap)
        {
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Upload_File\\" + tahap;

            // Periksa ukuran file sebelum mengirimkannya
            FileInfo fileInfo = new FileInfo(filepath);
            long fileSize = fileInfo.Length;
            long maxSize = 10 * 1024 * 1024; // 10 MB dalam byte

            if (fileSize > maxSize)
            {
                // Mengembalikan respons atau pesan yang sesuai jika ukuran file melebihi batas maksimum
                return Content("Ukuran file terlalu besar. Batas maksimum adalah 10 MB.");
            }

            return File(filepath, "application/octet-stream");
        }
    }
}