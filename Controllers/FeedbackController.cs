using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JourneySpire.Models;

namespace JourneySpire.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly string _connectionString;

        public FeedbackController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // =================================================
        // CUSTOMER – CREATE FEEDBACK (GET)
        // =================================================
        public IActionResult Create()
        {
            return View();
        }

        // =================================================
        // CUSTOMER – CREATE FEEDBACK (POST)
        // =================================================
        [HttpPost]
        public IActionResult Create(Feedback model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FullName", model.FullName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Message", model.Message);
                    cmd.Parameters.AddWithValue("@Rating", model.Rating);
                    cmd.Parameters.AddWithValue("@BookingId", model.BookingId ?? (object)DBNull.Value);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                TempData["Success"] = "Thank You For Your Feedback";
                return RedirectToAction("Index","Feedback");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // =================================================
        // CUSTOMER – THANK YOU PAGE
        // =================================================
        public IActionResult ThankYou()
        {
            return View();
        }

        // =================================================
        // ADMIN – VIEW ALL FEEDBACK
        // =================================================
        public IActionResult Index()
        {
            List<Feedback> list = new List<Feedback>();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_GetAll", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new Feedback
                        {
                            FeedbackId = Convert.ToInt32(dr["FeedbackId"]),
                            FullName = dr["FullName"].ToString(),
                            Email = dr["Email"].ToString(),
                            Message = dr["Message"].ToString(),
                            Rating = Convert.ToInt32(dr["Rating"]),
                            BookingId = dr["BookingId"] == DBNull.Value ? null : (int?)Convert.ToInt32(dr["BookingId"]),
                            IsApproved = Convert.ToBoolean(dr["IsApproved"]),
                            CreatedDate = Convert.ToDateTime(dr["CreatedDate"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View("~/Views/Feedback/Index.cshtml", list);
        }

        // =================================================
        // ADMIN – DETAILS
        // =================================================
        public IActionResult Details(int id)
        {
            Feedback model = new Feedback();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_GetById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackId", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.FeedbackId = Convert.ToInt32(dr["FeedbackId"]);
                        model.FullName = dr["FullName"].ToString();
                        model.Email = dr["Email"].ToString();
                        model.Message = dr["Message"].ToString();
                        model.Rating = Convert.ToInt32(dr["Rating"]);
                        model.IsApproved = Convert.ToBoolean(dr["IsApproved"]);
                        model.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View("~/Views/Feedback/Details.cshtml", model);
        }

        // =================================================
        // ADMIN – APPROVE / UPDATE
        // =================================================
        [HttpPost]
        public IActionResult Approve(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FeedbackId", id);
                    cmd.Parameters.AddWithValue("@IsApproved", 1);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // =================================================
        // ADMIN – DELETE (GET)
        // =================================================
        public IActionResult Delete(int id)
        {
            Feedback model = new Feedback();

            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_GetById", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackId", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        model.FeedbackId = Convert.ToInt32(dr["FeedbackId"]);
                        model.FullName = dr["FullName"].ToString();
                        model.Email = dr["Email"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return View("~/Views/Feedback/Delete.cshtml", model);
        }

        // =================================================
        // ADMIN – DELETE (POST)
        // =================================================
        [HttpPost]
        public IActionResult DeleteConfirmed(int feedbackId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("sp_Feedback_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FeedbackId", feedbackId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}
