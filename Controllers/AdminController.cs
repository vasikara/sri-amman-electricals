using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using JourneySpire.Models;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace JourneySpire.Controllers
{
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnection()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Home()
        {
            return View();
        }

        // =========================
        // LOGIN
        // =========================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string Email, string Password)
        {
            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", Email);
                cmd.Parameters.AddWithValue("@Password", Password);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    HttpContext.Session.SetInt32("AdminId", (int)dr["AdminId"]);
                    HttpContext.Session.SetString("AdminEmail", dr["Email"].ToString());
                    return RedirectToAction("Index","Admin");
                }

                ViewBag.Error = "Invalid Email or Password";
                return View();
            }
        }

        // =========================
        // LOGOUT
        // =========================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // =========================
        // DASHBOARD (HOME)
        // =========================
        public IActionResult Dashboard()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return View();
        }

        // =========================
        // ADMIN CRUD
        // =========================
        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            List<AdminModel> list = new List<AdminModel>();

            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    list.Add(new AdminModel
                    {
                        AdminId = (int)rdr["AdminId"],
                        FullName = rdr["FullName"].ToString(),
                        Email = rdr["Email"].ToString(),
                        Role = rdr["Role"].ToString(),
                        Phone = rdr["Phone"].ToString()
                    });
                }
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdminModel admin)
        {
            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", admin.FullName);
                cmd.Parameters.AddWithValue("@Email", admin.Email);
                cmd.Parameters.AddWithValue("@Password", admin.Password);
                cmd.Parameters.AddWithValue("@Role", admin.Role);
                cmd.Parameters.AddWithValue("@Phone", admin.Phone);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Registration Successful. Please Login.";
            return RedirectToAction("Login");
        }

        public IActionResult Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            AdminModel admin = new AdminModel();

            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    admin.AdminId = id;
                    admin.FullName = dr["FullName"].ToString();
                    admin.Email = dr["Email"].ToString();
                    admin.Role = dr["Role"].ToString();
                    admin.Phone = dr["Phone"].ToString();
                }
            }
            return View(admin);
        }

        [HttpPost]
        public IActionResult Edit(AdminModel admin)
        {
            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@AdminId", admin.AdminId);
                cmd.Parameters.AddWithValue("@FullName", admin.FullName);
                cmd.Parameters.AddWithValue("@Role", admin.Role);
                cmd.Parameters.AddWithValue("@Phone", admin.Phone);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(GetConnection()))
            {
                SqlCommand cmd = new SqlCommand("sp_Admin_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdminId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }


        public IActionResult ForgotPassword()
        {
            return View();
        }
        // =========================
        // ALL MODULE REDIRECTS
        // =========================
        public IActionResult TourPackages()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "TourPackage");
        }

        public IActionResult Bookings()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Booking");
        }

        public IActionResult Customers()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Vehicles()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Vehicle");
        }

        public IActionResult Guides()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Guide");
        }

        public IActionResult Payments()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Payment");
        }

        public IActionResult Feedback()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Feedback");
        }

        public IActionResult Reports()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");
            return RedirectToAction("Index", "Report");
        }

        // =========================
        // SESSION CHECK
        // =========================
        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("AdminEmail") != null;
        }
    }
}
