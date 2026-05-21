using JourneySpire.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;

namespace JourneySpire.Controllers
{
    public class GuideController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public GuideController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            List<Guide> guides = new List<Guide>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Guide_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        guides.Add(new Guide
                        {
                            GuideId = Convert.ToInt32(rdr["GuideId"]),
                            FullName = rdr["FullName"].ToString(),
                            Phone = rdr["Phone"].ToString(),
                            Email = rdr["Email"].ToString(),
                            Language = rdr["Language"].ToString(),
                            Experience = Convert.ToInt32(rdr["Experience"]),
                            Photo = rdr["Photo"].ToString(),
                            Status = rdr["Status"].ToString(),
                            IsAvailable = Convert.ToBoolean(rdr["IsAvailable"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(guides);
        }

        // ===================== CREATE =====================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Guide guide)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Guide_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", guide.FullName);
                    cmd.Parameters.AddWithValue("@Phone", guide.Phone);
                    cmd.Parameters.AddWithValue("@Email", guide.Email);
                    cmd.Parameters.AddWithValue("@Language", guide.Language);
                    cmd.Parameters.AddWithValue("@Experience", guide.Experience);
                    cmd.Parameters.AddWithValue("@Photo", guide.Photo ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Create","Vehicle");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(guide);
            }
        }

        // ===================== EDIT =====================
        public IActionResult Edit(int id)
        {
            Guide guide = new Guide();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Guide_GetById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GuideId", id);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        guide.GuideId = Convert.ToInt32(rdr["GuideId"]);
                        guide.FullName = rdr["FullName"].ToString();
                        guide.Phone = rdr["Phone"].ToString();
                        guide.Email = rdr["Email"].ToString();
                        guide.Language = rdr["Language"].ToString();
                        guide.Experience = Convert.ToInt32(rdr["Experience"]);
                        guide.Photo = rdr["Photo"].ToString();
                        guide.Status = rdr["Status"].ToString();
                        guide.IsAvailable = Convert.ToBoolean(rdr["IsAvailable"]);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(guide);
        }

        [HttpPost]
        public IActionResult Edit(Guide guide)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Guide_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@GuideId", guide.GuideId);
                    cmd.Parameters.AddWithValue("@FullName", guide.FullName);
                    cmd.Parameters.AddWithValue("@Phone", guide.Phone);
                    cmd.Parameters.AddWithValue("@Email", guide.Email);
                    cmd.Parameters.AddWithValue("@Language", guide.Language);
                    cmd.Parameters.AddWithValue("@Experience", guide.Experience);
                    cmd.Parameters.AddWithValue("@Photo", guide.Photo ?? "");
                    cmd.Parameters.AddWithValue("@Status", guide.Status);
                    cmd.Parameters.AddWithValue("@IsAvailable", guide.IsAvailable);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(guide);
            }
        }

        // ===================== DELETE =====================
        public IActionResult Delete(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Guide_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@GuideId", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ===================== AVAILABLE GUIDES (BOOKING) =====================
        public IActionResult AvailableGuides()
        {
            List<Guide> guides = new List<Guide>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_Guide_GetAvailable", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    guides.Add(new Guide
                    {
                        GuideId = Convert.ToInt32(rdr["GuideId"]),
                        FullName = rdr["FullName"].ToString()
                    });
                }
            }

            return Json(guides);
        }
    }
}
