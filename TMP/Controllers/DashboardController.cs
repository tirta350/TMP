using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TMP.Models;

namespace TMP.Controllers
{
    public class DashboardController : Controller
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Dashboard_Admin dashboard_Admin = new Dashboard_Admin();
        Proyek _proyek = new Proyek();

        public ActionResult Dashboard_Admin()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                int totalProjects = dashboard_Admin.GetTotalProjects();
                int onprogressProjects = dashboard_Admin.GetOnProgressProjects();
                int NeedAttentionProject = dashboard_Admin.GetNeedAttentionProject();

                // Simpan nilai totalProjects ke dalam model atau ViewBag
                ViewBag.TotalProjects = totalProjects;
                ViewBag.OnProgressProjects = onprogressProjects;
                ViewBag.NeedAttentionProject = NeedAttentionProject;

                return View();
            }
        }

        public ActionResult Dashboard_Dosen()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                UserModel user = (UserModel)Session["user"];
                int totalProjects = dashboard_Admin.GetTotalProjectsLecturer(user);
                int onprogressProjects = dashboard_Admin.GetOnProgressProjectsLecturer(user);
                int NeedAttentionProject = dashboard_Admin.GetNeedAttentionProjectLecturer(user);

                // Simpan nilai totalProjects ke dalam model atau ViewBag
                ViewBag.TotalProjects = totalProjects;
                ViewBag.OnProgressProjects = onprogressProjects;
                ViewBag.NeedAttentionProject = NeedAttentionProject;

                return View();
            }
        }

        public JsonResult getData()
        {
            var chart = new List<ChartModel>();
            con.Open();
            using (var command = new SqlCommand("SELECT nama_proyek, progress FROM proyek WHERE status != '0'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartData(string pic)
        {
            var chart = new List<ChartModel>();
            con.Open();
            using (var command = new SqlCommand("SELECT nama_proyek, progress FROM proyek WHERE status != '0' AND pic LIKE '" + pic + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartDataWithDateRange(string tanggalAwal, string tanggalAkhir)
        {
            var chart = new List<ChartModel>();
            con.Open();
            using (var command = new SqlCommand("SELECT nama_proyek, progress FROM proyek WHERE status != '0' AND target BETWEEN @tanggalAwal AND @tanggalAkhir", con))
            {
                command.Parameters.AddWithValue("@tanggalAwal", tanggalAwal);
                command.Parameters.AddWithValue("@tanggalAkhir", tanggalAkhir);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        chart.Add(new ChartModel
                        {
                            nama_proyek = reader.GetString(0),
                            progress = reader.GetInt32(1)
                        });
                    }
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDataTahunDosen()
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand("Select p.nama_proyek as nama_proyek, p.progress as progress from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel WHERE p.status != 0" +
                "AND p.pic LIKE '" + namaUser + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartDataTahunDosen(string tahun)
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand("Select p.nama_proyek as nama_proyek, p.progress as progress from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel WHERE p.status != 0" +
                "AND p.pic LIKE '" + namaUser + "' AND YEAR(p.tanggal_mulai) LIKE '" + tahun + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartDataWithDateRangeDosen(string tanggalAwal, string tanggalAkhir)
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand("SELECT p.nama_proyek AS nama_proyek, p.progress AS progress FROM proyek AS p " +
                "JOIN kelompok AS k ON p.id_kel = k.id_kel WHERE p.status != 0 " +
                "AND p.pic LIKE '" + namaUser + "' " +
                "AND p.tanggal_mulai BETWEEN '" + tanggalAwal + "' AND '" + tanggalAkhir + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Dashboard_Mahasiswa()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                UserModel user = (UserModel)Session["user"];
                int totalProjects = dashboard_Admin.GetTotalProjectsStudent(user);
                int onprogressProjects = dashboard_Admin.GetOnProgressProjectsStudent(user);
                int NeedAttentionProject = dashboard_Admin.GetNeedAttentionProjectStudent(user);

                // Simpan nilai totalProjects ke dalam model atau ViewBag
                ViewBag.TotalProjects = totalProjects;
                ViewBag.OnProgressProjects = onprogressProjects;
                ViewBag.NeedAttentionProject = NeedAttentionProject;

                return View();
            }
        }

        public JsonResult getDataTahunMhs()
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand(@"Select p.nama_proyek as nama_proyek, p.progress as progress from proyek as p JOIN detail_kelompok as dk on p.id_kel = dk.id_kel WHERE p.status != 0" +
                "AND dk.nama_anggota LIKE '" + namaUser + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartDataTahunMhs(string tahun)
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand("Select p.nama_proyek as nama_proyek, p.progress as progress from proyek as p JOIN kelompok as k on p.id_kel = k.id_kel " +
                "JOIN detail_kelompok as dk on k.id_kel = dk.id_kel WHERE p.status != 0 AND dk.nama_anggota LIKE '" + namaUser + "' AND YEAR(p.tanggal_mulai) LIKE '" + tahun + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterChartDataWithDateRangeMahasiswa(string tanggalAwal, string tanggalAkhir)
        {
            var chart = new List<ChartModel>();
            UserModel user = (UserModel)Session["user"];
            string namaUser = user.nama_user;

            con.Open();
            using (var command = new SqlCommand("SELECT p.nama_proyek AS nama_proyek, p.progress AS progress FROM proyek AS p " +
                "JOIN kelompok AS k ON p.id_kel = k.id_kel " +
                "JOIN detail_kelompok AS dk ON k.id_kel = dk.id_kel " +
                "WHERE p.status != 0 AND dk.nama_anggota LIKE '" + namaUser + "' " +
                "AND p.tanggal_mulai BETWEEN '" + tanggalAwal + "' AND '" + tanggalAkhir + "'", con))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    chart.Add(new ChartModel
                    {
                        nama_proyek = reader.GetString(0),
                        progress = reader.GetInt32(1)
                    });
                }
            }
            con.Close();

            // membuat objek JSON untuk data chart
            var chartData = new
            {
                labels = chart.Select(sd => sd.nama_proyek),
                data = chart.Select(sd => sd.progress),
            };

            // mengirim objek JSON ke view
            return Json(chartData, JsonRequestBehavior.AllowGet);
        }

        // GET: Section
        public ActionResult EntireProject()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            //UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.EntireProject());
        }

        // GET: Section
        public ActionResult OnProgressProject()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            //UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.OnProgressProject());
        }

        // GET: Section
        public ActionResult NeedAttentionProject()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            //UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.NeedAttentionProject());
        }

        // GET: Section
        public ActionResult EntireProjectLecturer()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.EntireProjectLecturer(user));
        }

        // GET: Section
        public ActionResult OnProgressProjectLecturer()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.OnProgressProjectLecturer(user));
        }

        // GET: Section
        public ActionResult NeedAttentionProjectLecturer()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.NeedAttentionProjectLecturer(user));
        }

        // GET: Section
        public ActionResult EntireProjectStudent()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.EntireProjectStudent(user));
        }

        // GET: Section
        public ActionResult OnProgressProjectStudent()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.OnProgressProjectStudent(user));
        }

        // GET: Section
        public ActionResult NeedAttentionProjectStudent()
        {
            if (Session["user"] == null)
            {
                Response.Write("<script>alert('Pesan: " + "Session Time Out!" + "');</script>");
                return RedirectToAction("User", "Login");
            }
            UserModel user = (UserModel)Session["user"];
            return View(dashboard_Admin.NeedAttentionProjectStudent(user));
        }
    }
}