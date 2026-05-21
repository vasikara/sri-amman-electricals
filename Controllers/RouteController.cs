using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using JourneySpire.Models;
using Route = JourneySpire.Models.Route;

namespace JourneySpire.Controllers
{
    public class RouteController : Controller
    {
        private readonly IConfiguration _configuration;

        public RouteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            List<Route> routes = new List<Route>();

            try
            {
                using (SqlConnection con =
                    new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Routes", con);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        routes.Add(new Route
                        {
                            RouteId = Convert.ToInt32(rdr["RouteId"]),
                            SourceLocation = rdr["SourceLocation"].ToString(),
                            DestinationLocation = rdr["DestinationLocation"].ToString(),
                            DistanceKm = Convert.ToInt32(rdr["DistanceKm"]),
                            EstimatedTime = rdr["EstimatedTime"].ToString(),
                            IsActive = Convert.ToBoolean(rdr["IsActive"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(routes);
        }

        // ================= CREATE GET =================
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST 
        [HttpPost]
        public IActionResult Create(Route model)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"INSERT INTO Routes 
                          (SourceLocation, DestinationLocation, DistanceKm, EstimatedTime, IsActive)
                          VALUES 
                          (@SourceLocation, @DestinationLocation, @DistanceKm, @EstimatedTime, 1)", con);

                    cmd.Parameters.AddWithValue("@SourceLocation", model.SourceLocation);
                    cmd.Parameters.AddWithValue("@DestinationLocation", model.DestinationLocation);
                    cmd.Parameters.AddWithValue("@DistanceKm", model.DistanceKm);
                    cmd.Parameters.AddWithValue("@EstimatedTime", model.EstimatedTime); 

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["Success"] = "Route added successfully";
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
            Route route = new Route();

            try
            {
                using (SqlConnection con =
                    new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT * FROM Routes WHERE RouteId=@RouteId", con);
                    cmd.Parameters.AddWithValue("@RouteId", id);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        route.RouteId = Convert.ToInt32(rdr["RouteId"]);
                        route.SourceLocation = rdr["SourceLocation"].ToString();
                        route.DestinationLocation = rdr["DestinationLocation"].ToString();
                        route.DistanceKm = Convert.ToInt32(rdr["DistanceKm"]);
                        route.EstimatedTime = rdr["EstimatedTime"].ToString();
                        route.IsActive = Convert.ToBoolean(rdr["IsActive"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(route);
        }

        // ================= EDIT POST =================
        [HttpPost]
        public IActionResult Edit(Route model)
        {
            try
            {
                using (SqlConnection con =
                    new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand(
                        @"UPDATE Routes SET 
                          SourceLocation=@SourceLocation,
                          DestinationLocation=@DestinationLocation,
                          DistanceKm=@DistanceKm,
                          EstimatedTime=@EstimatedTime
                          WHERE RouteId=@RouteId", con);

                    cmd.Parameters.AddWithValue("@RouteId", model.RouteId);
                    cmd.Parameters.AddWithValue("@SourceLocation", model.SourceLocation);
                    cmd.Parameters.AddWithValue("@DestinationLocation", model.DestinationLocation);
                    cmd.Parameters.AddWithValue("@DistanceKm", model.DistanceKm);
                    cmd.Parameters.AddWithValue("@EstimatedTime", model.EstimatedTime);

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
                using (SqlConnection con =
                    new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd =
                        new SqlCommand("DELETE FROM Routes WHERE RouteId=@RouteId", con);
                    cmd.Parameters.AddWithValue("@RouteId", id);

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
