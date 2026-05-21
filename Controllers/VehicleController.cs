using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using JourneySpire.Models;

namespace JourneySpire.Controllers
{
    public class VehicleController : Controller
    {
        private readonly IConfiguration _configuration;

        public VehicleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            List<Vehicle> vehicles = new List<Vehicle>();

            try
            {
                using (SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Vehicles", con);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        vehicles.Add(new Vehicle
                        {
                            VehicleId = Convert.ToInt32(rdr["VehicleId"]),
                            VehicleNumber = rdr["VehicleNumber"].ToString(),
                            VehicleType = rdr["VehicleType"].ToString(),
                            Capacity = Convert.ToInt32(rdr["Capacity"]),
                            IsAvailable = Convert.ToBoolean(rdr["IsAvailable"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(vehicles);
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            return View();
        }

        // ================= CREATE POST =================
        [HttpPost]
        public IActionResult Create(Vehicle model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Vehicles 
                          (VehicleNumber, VehicleType, Capacity, IsAvailable)
                          VALUES 
                          (@VehicleNumber, @VehicleType, @Capacity, 1)", con);

                    cmd.Parameters.AddWithValue("@VehicleNumber", model.VehicleNumber);
                    cmd.Parameters.AddWithValue("@VehicleType", model.VehicleType);
                    cmd.Parameters.AddWithValue("@Capacity", model.Capacity);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["Success"] = "Vehicle added successfully";
                return RedirectToAction("Create","Payment");
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
            Vehicle vehicle = new Vehicle();

            try
            {
                using (SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM Vehicles WHERE VehicleId=@VehicleId", con);
                    cmd.Parameters.AddWithValue("@VehicleId", id);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        vehicle.VehicleId = Convert.ToInt32(rdr["VehicleId"]);
                        vehicle.VehicleNumber = rdr["VehicleNumber"].ToString();
                        vehicle.VehicleType = rdr["VehicleType"].ToString();
                        vehicle.Capacity = Convert.ToInt32(rdr["Capacity"]);
                        vehicle.IsAvailable = Convert.ToBoolean(rdr["IsAvailable"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(vehicle);
        }

        // ================= EDIT POST =================
        [HttpPost]
        public IActionResult Edit(Vehicle model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Vehicles SET 
                          VehicleNumber=@VehicleNumber,
                          VehicleType=@VehicleType,
                          Capacity=@Capacity,
                          IsAvailable=@IsAvailable
                          WHERE VehicleId=@VehicleId", con);

                    cmd.Parameters.AddWithValue("@VehicleId", model.VehicleId);
                    cmd.Parameters.AddWithValue("@VehicleNumber", model.VehicleNumber);
                    cmd.Parameters.AddWithValue("@VehicleType", model.VehicleType);
                    cmd.Parameters.AddWithValue("@Capacity", model.Capacity);
                    cmd.Parameters.AddWithValue("@IsAvailable", model.IsAvailable);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

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
                using (SqlConnection con = new SqlConnection(
                    _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd =
                        new SqlCommand("DELETE FROM Vehicles WHERE VehicleId=@VehicleId", con);
                    cmd.Parameters.AddWithValue("@VehicleId", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
