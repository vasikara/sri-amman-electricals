using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JourneySpire.Models;

namespace JourneySpire.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IConfiguration _configuration;
        private object PaymentId;

        public PaymentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            List<Payment> list = new List<Payment>();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Payments_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new Payment
                    {
                        PaymentId = Convert.ToInt32(dr["PaymentId"]),
                        BookingId = Convert.ToInt32(dr["BookingId"]),
                        CustomerName = dr["CustomerName"].ToString(),
                        PackageName = dr["PackageName"].ToString(),
                        Amount = Convert.ToDecimal(dr["Amount"]),
                        PaymentMode = dr["PaymentMode"].ToString(),
                        PaymentStatus = dr["PaymentStatus"].ToString(),
                        TransactionId = dr["TransactionId"].ToString(),
                        PaymentDate = Convert.ToDateTime(dr["PaymentDate"])
                    });
                }
            }
            return View(list);
        }

        // ===================== CREATE (GET) =====================
        public IActionResult Create(int bookingId)
        {
            Payment model = new Payment();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Booking_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingId", bookingId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.BookingId = bookingId;
                    model.CustomerName = dr["CustomerName"].ToString();
                    model.PackageName = dr["PackageName"].ToString();
                    model.Amount = Convert.ToDecimal(dr["TotalAmount"]);
                }
            }
            return View(model);
        }

        // ===================== CREATE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Payment model)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Payments_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@BookingId", model.BookingId);
                    cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@Amount", model.Amount);
                    cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                    cmd.Parameters.AddWithValue("@PaymentStatus", "Paid");
                    cmd.Parameters.AddWithValue("@TransactionId", Guid.NewGuid().ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                return RedirectToAction("Index","Home");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // EDIT

        [HttpGet]
        public IActionResult Edit(int bookingId)
        {
            Payment model = new Payment();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Payment_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", PaymentId);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cmd.Parameters.AddWithValue("@PaymentId", model.PaymentId);
                    cmd.Parameters.AddWithValue("@BookingId", model.BookingId);
                    cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@Amount", model.Amount);
                    cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                    cmd.Parameters.AddWithValue("@PaymentStatus", "Paid");
                    cmd.Parameters.AddWithValue("@TransactionId", Guid.NewGuid().ToString());

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Payment model)
        {
            try
            {
                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Payments_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@BookingId", model.BookingId);
                    cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@Amount", model.Amount);
                    cmd.Parameters.AddWithValue("@PaymentMode", model.PaymentMode);
                    cmd.Parameters.AddWithValue("@PaymentStatus", "Paid");
                    cmd.Parameters.AddWithValue("@TransactionId", Guid.NewGuid().ToString());

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

        // ===================== DETAILS =====================
        public IActionResult Details(int id)
        {
            Payment model = new Payment();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Payments_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.PaymentId = id;
                    model.CustomerName = dr["CustomerName"].ToString();
                    model.PackageName = dr["PackageName"].ToString();
                    model.Amount = Convert.ToDecimal(dr["Amount"]);
                    model.PaymentMode = dr["PaymentMode"].ToString();
                    model.PaymentStatus = dr["PaymentStatus"].ToString();
                    model.TransactionId = dr["TransactionId"].ToString();
                    model.PaymentDate = Convert.ToDateTime(dr["PaymentDate"]);
                }
            }
            return View(model);
        }

        // ===================== DELETE (GET) =====================
        public IActionResult Delete(int id)
        {
            Payment model = new Payment();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Payments_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    model.PaymentId = id;
                    model.CustomerName = dr["CustomerName"].ToString();
                    model.PackageName = dr["PackageName"].ToString();
                    model.Amount = Convert.ToDecimal(dr["Amount"]);
                }
            }
            return View(model);
        }

        // ===================== DELETE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int paymentId)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Payments_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaymentId", paymentId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
