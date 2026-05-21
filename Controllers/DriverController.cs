using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using JourneySpire.Models;
using System.Data.SqlClient;

namespace JourneySpire.Controllers
{
    public class DriverController : Controller
    {
        private readonly IConfiguration _configuration;

        public DriverController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            List<Driver> list = new List<Driver>();

            try
            {
                using SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand("SELECT * FROM Drivers", con);
                con.Open();

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new Driver
                    {
                        DriverId = (int)rdr["DriverId"],
                        DriverName = rdr["DriverName"].ToString(),
                        Phone = rdr["Phone"].ToString(),
                        LicenseNumber = rdr["LicenseNumber"].ToString(),
                        IsAvailable = Convert.ToBoolean(rdr["IsAvailable"])
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(list);
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        public IActionResult Create(Driver model)
        {
            try
            {
                using SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand(
                    @"INSERT INTO Drivers
                      (DriverName, Phone, LicenseNumber, IsAvailable)
                      VALUES (@DriverName, @Phone, @LicenseNumber, @IsAvailable)", con);

                cmd.Parameters.AddWithValue("@DriverName", model.DriverName);
                cmd.Parameters.AddWithValue("@Phone", model.Phone);
                cmd.Parameters.AddWithValue("@LicenseNumber", model.LicenseNumber);
                cmd.Parameters.AddWithValue("@IsAvailable", model.IsAvailable);

                con.Open();
                cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ================= EDIT GET =================
        public IActionResult Edit(int id)
        {
            Driver driver = new Driver();

            try
            {
                using SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Drivers WHERE DriverId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    driver.DriverId = (int)rdr["DriverId"];
                    driver.DriverName = rdr["DriverName"].ToString();
                    driver.Phone = rdr["Phone"].ToString();
                    driver.LicenseNumber = rdr["LicenseNumber"].ToString();
                    driver.IsAvailable = Convert.ToBoolean(rdr["IsAvailable"]);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(driver);
        }

        // ================= EDIT POST =================
        [HttpPost]
        public IActionResult Edit(Driver model)
        {
            try
            {
                using SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand(
                    @"UPDATE Drivers SET
                      DriverName=@DriverName,
                      Phone=@Phone,
                      LicenseNumber=@LicenseNumber,
                      IsAvailable=@IsAvailable
                      WHERE DriverId=@DriverId", con);

                cmd.Parameters.AddWithValue("@DriverId", model.DriverId);
                cmd.Parameters.AddWithValue("@DriverName", model.DriverName);
                cmd.Parameters.AddWithValue("@Phone", model.Phone);
                cmd.Parameters.AddWithValue("@LicenseNumber", model.LicenseNumber);
                cmd.Parameters.AddWithValue("@IsAvailable", model.IsAvailable);

                con.Open();
                cmd.ExecuteNonQuery();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            try
            {
                using SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection"));

                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Drivers WHERE DriverId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
