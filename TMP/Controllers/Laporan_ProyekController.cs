using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TMP.Models;

namespace TMP.Controllers
{
    public class Laporan_ProyekController : Controller
    {
        Laporan_Proyek laporan_proyek = new Laporan_Proyek();
        // GET: Laporan_Proyek
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                return View(laporan_proyek.getAllData());
            }
        }

        public ActionResult Cetak_Laporan(string tanggalAwal, string tanggalAkhir)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                // Convert the date strings to DateTime objects for filtering
                DateTime tanggalAwalDate;
                DateTime tanggalAkhirDate;

                // Specify the date format expected in the input strings
                string dateFormat = "yyyy-MM-dd";

                // Try parsing the input strings using the specified date format
                if (!DateTime.TryParseExact(tanggalAwal, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAwalDate)
                    || !DateTime.TryParseExact(tanggalAkhir, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAkhirDate))
                {
                    // Handle the case where the date strings are not in the correct format
                    // You can redirect the user to an error page or display an error message
                    // For example:
                    ModelState.AddModelError("", "Invalid date format. Please enter dates in the format yyyy-MM-dd.");
                    return View();
                }

                // Get the data from the database using the filtered date range
                // Assuming you have a method named getAllDataLaporanByDateRange that accepts the date range as parameters and returns the filtered data
                IList<Laporan_Proyek_Model> lap = laporan_proyek.getAllDataLaporanByDateRange(tanggalAwalDate, tanggalAkhirDate);

                return View(lap);
            }
        }

        public ActionResult Index_Lap_Dosen()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                UserModel user = (UserModel)Session["user"];
                return View(laporan_proyek.getAllDataDosen(user));
            }
        }

        public ActionResult Cetak_Laporan_Dosen(string tanggalAwal, string tanggalAkhir, string namaUser)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                // Convert the date strings to DateTime objects for filtering
                DateTime tanggalAwalDate;
                DateTime tanggalAkhirDate;

                // Specify the date format expected in the input strings
                string dateFormat = "yyyy-MM-dd";

                // Try parsing the input strings using the specified date format
                if (!DateTime.TryParseExact(tanggalAwal, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAwalDate)
                    || !DateTime.TryParseExact(tanggalAkhir, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAkhirDate))
                {
                    // Handle the case where the date strings are not in the correct format
                    // You can redirect the user to an error page or display an error message
                    // For example:
                    ModelState.AddModelError("", "Invalid date format. Please enter dates in the format yyyy-MM-dd.");
                    return View();
                }

                // Get the data from the database using the filtered date range
                // Assuming you have a method named getAllDataLaporanByDateRangeDosen that accepts the date range as parameters and returns the filtered data
                UserModel user = (UserModel)Session["user"];
                IList<Laporan_Proyek_Model> lap = laporan_proyek.getAllDataLaporanByDateRangeDosen(user, tanggalAwalDate, tanggalAkhirDate);

                return View(lap);
            }
        }

        public ActionResult Index_Lap_Mahasiswa()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                UserModel user = (UserModel)Session["user"];
                return View(laporan_proyek.getAllDataMahasiswa(user));
            }
        }

        public ActionResult Cetak_Laporan_Mahasiswa(string tanggalAwal, string tanggalAkhir, string namaUser)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }
            else
            {
                // Check if both tanggalAwal and tanggalAkhir are not empty
                if (string.IsNullOrEmpty(tanggalAwal) || string.IsNullOrEmpty(tanggalAkhir))
                {
                    // Handle the case where one or both dates are empty
                    // You can redirect the user to an error page or display an error message
                    // For example:
                    ModelState.AddModelError("", "Both tanggalAwal and tanggalAkhir are required for filtering.");
                    return View();
                }

                // Convert the date strings to DateTime objects for filtering
                DateTime tanggalAwalDate;
                DateTime tanggalAkhirDate;

                // Specify the date format expected in the input strings
                string dateFormat = "yyyy-MM-dd";

                // Try parsing the input strings using the specified date format
                if (!DateTime.TryParseExact(tanggalAwal, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAwalDate)
                    || !DateTime.TryParseExact(tanggalAkhir, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out tanggalAkhirDate))
                {
                    // Handle the case where the date strings are not in the correct format
                    // You can redirect the user to an error page or display an error message
                    // For example:
                    ModelState.AddModelError("", "Invalid date format. Please enter dates in the format yyyy-MM-dd.");
                    return View();
                }

                // Get the data from the database using the filtered date range
                // Assuming you have a method named getAllDataLaporanByDateRangeMahasiswa that accepts the date range as parameters and returns the filtered data
                UserModel user = (UserModel)Session["user"];
                IList<Laporan_Proyek_Model> lap = laporan_proyek.getAllDataLaporanByDateRangeMahasiswa(user, tanggalAwalDate, tanggalAkhirDate);

                return View(lap);
            }
        }
    }
}