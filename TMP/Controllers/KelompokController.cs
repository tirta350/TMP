using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TMP.Models;

namespace TMP.Controllers
{
    public class KelompokController : Controller
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        Kelompok kelompok = new Kelompok();
        private Kelompok db = new Kelompok();
        private User user = new User();
        List<cart> li = new List<cart>();

        // GET: Kelompok
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            return View(kelompok.getAllData());
        }

        private async Task<List<UserModel>> GetDataP4FromAPI()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync("https://api.polytechnic.astra.ac.id:2906/api_dev/efcc359990d14328fda74beb65088ef9660ca17e/SIA/getListMahasiswa?id_konsentrasi=1");
                return JsonConvert.DeserializeObject<List<UserModel>>(result);
            }
        }

        private async Task<List<UserModel>> GetDataTPMFromAPI()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync("https://api.polytechnic.astra.ac.id:2906/api_dev/efcc359990d14328fda74beb65088ef9660ca17e/SIA/getListMahasiswa?id_konsentrasi=2");
                return JsonConvert.DeserializeObject<List<UserModel>>(result);
            }
        }

        private async Task<List<UserModel>> GetDataMKFromAPI()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync("https://api.polytechnic.astra.ac.id:2906/api_dev/efcc359990d14328fda74beb65088ef9660ca17e/SIA/getListMahasiswa?id_konsentrasi=5");
                return JsonConvert.DeserializeObject<List<UserModel>>(result);
            }
        }

        // GET: kelompoks/Create
        public async Task<ActionResult> Create()
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
                    List<cart> li2 = TempData["cart"] as List<cart>;
                    int ada = 0;
                    foreach (var item in li2)
                    {

                    }
                    Session["C"] = ada;
                }
                TempData.Keep();
                //List<UserModel> data = await GetDataP4FromAPI();
                UserKelompokModel userKelompokModel = user.getall();
                userKelompokModel.kelompok = new KelompokModel();
                return PartialView(userKelompokModel);
            }
        }

        // GET: kelompoks/Create
        public async Task<ActionResult> Create_TPM()
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
                    List<cart> li2 = TempData["cart"] as List<cart>;
                    int ada = 0;
                    foreach (var item in li2)
                    {

                    }
                    Session["C"] = ada;
                }
                TempData.Keep();
                List<UserModel> data = await GetDataTPMFromAPI();
                //UserKelompokModel userKelompokModel = user.getall();
                //userKelompokModel.kelompok = new KelompokModel();
                return PartialView(data);
            }
        }

        // GET: kelompoks/Create
        public async Task<ActionResult> Create_P4()
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
                    List<cart> li2 = TempData["cart"] as List<cart>;
                    int ada = 0;
                    foreach (var item in li2)
                    {

                    }
                    Session["C"] = ada;
                }
                TempData.Keep();
                List<UserModel> data = await GetDataP4FromAPI();
                //UserKelompokModel userKelompokModel = user.getall();
                //userKelompokModel.kelompok = new KelompokModel();
                return PartialView(data);
            }
        }

        // GET: kelompoks/Create
        public async Task<ActionResult> Create_MK()
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
                    List<cart> li2 = TempData["cart"] as List<cart>;
                    int ada = 0;
                    foreach (var item in li2)
                    {

                    }
                    Session["C"] = ada;
                }
                TempData.Keep();
                List<UserModel> data = await GetDataMKFromAPI();
                //UserKelompokModel userKelompokModel = user.getall();
                //userKelompokModel.kelompok = new KelompokModel();
                return PartialView(data);
            }
        }

        // POST: kelompoks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserModel model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();
            //UserModel detil = user.getData(model.id);
            //if (detil == null)
            //{
            //    TempData["ErrorMessage"] = "Data Tidak Ditemukan";
            //    return RedirectToAction("Create");
            //}
            //else
            //{

            c.nim = model.nim;
            c.nama = model.nama;

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
                    if (item.nim == c.nim)
                    {
                        TempData["ErrorMessage"] = "Data Telah Ditambahkan";
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
            //}

            return RedirectToAction("Create");
        }

        // POST: kelompoks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_TPM(UserModel model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();
            //UserModel detil = user.getData(model.id);
            //if (detil == null)
            //{
            //    TempData["ErrorMessage"] = "Data Tidak Ditemukan";
            //    return RedirectToAction("Create");
            //}
            //else
            //{

            c.nim = model.nim;
            c.nama = model.nama;

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
                    if (item.nim == c.nim)
                    {
                        TempData["ErrorMessage"] = "Data Telah Ditambahkan";
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
            //}

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_P4(UserModel model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();
            //UserModel detil = user.getData(model.id);
            //if (detil == null)
            //{
            //    TempData["ErrorMessage"] = "Data Tidak Ditemukan";
            //    return RedirectToAction("Create");
            //}
            //else
            //{

            c.nim = model.nim;
            c.nama = model.nama;

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
                    if (item.nim == c.nim)
                    {
                        TempData["ErrorMessage"] = "Data Telah Ditambahkan";
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
            //}

            return RedirectToAction("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_MK(UserModel model)
        {
            TempData["SuccessMessage"] = "";
            TempData["EmptyMessage"] = "";
            TempData["ErrorMessage"] = "";
            cart c = new cart();
            //UserModel detil = user.getData(model.id);
            //if (detil == null)
            //{
            //    TempData["ErrorMessage"] = "Data Tidak Ditemukan";
            //    return RedirectToAction("Create");
            //}
            //else
            //{

            c.nim = model.nim;
            c.nama = model.nama;

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
                    if (item.nim == c.nim)
                    {
                        TempData["ErrorMessage"] = "Data Telah Ditambahkan";
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
            //}

            return RedirectToAction("Create");
        }

        public ActionResult Clear_Cart(string id)
        {
            var items = TempData["cart"] as List<cart>;
            var item = items.FirstOrDefault(i => i.nim == id); // mencari item di dalam list

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
        public ActionResult Save(KelompokModel _kel, int id, cart c)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("User", "Login");
            }

            if (TempData["cart"] != null || _kel.nama_kel.Equals(""))
            {
                List<cart> li = TempData["cart"] as List<cart>;
                int primaryKey;

                SqlCommand cmd = new SqlCommand("spkelompokinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nama_kel", _kel.nama_kel);
                cmd.Parameters.AddWithValue("@tahun", DateTime.Now.Year.ToString());
                cmd.Parameters.AddWithValue("@status", 1);

                con.Open();
                primaryKey = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                // Cari dulu id dari kelompok yang terakhir diinsert
                // baru di looping insert detail

                foreach (var item in li)
                {
                    UserModel detil = user.getData(item.id);
                    SqlCommand cmd2 = new SqlCommand("spdetailkelinsert", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@id_user", item.nim);
                    cmd2.Parameters.AddWithValue("@id_kel", primaryKey);
                    cmd2.Parameters.AddWithValue("@nama_anggota", item.nama);
                    cmd2.Parameters.AddWithValue("@status", 1);
                    con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                }
                TempData.Remove("cart");

                TempData["message"] = "Data Kelompok Berhasil di Tambahkan";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Harap Isi Semua Data!";
                return RedirectToAction("Create");

            }

            
        }

        public ActionResult Delete(int id, KelompokModel kelompokModel)
        {
            // TODO: Add delete logic here
            SqlCommand cmd = new SqlCommand("spkeldelete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id_kel", id);
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

            return View(kelompok.getAllDataDetail(id));
        }

        public PartialViewResult nama_kel()
        {
            UserModel userModel = new UserModel();
            return PartialView(userModel);
        }
    }
}