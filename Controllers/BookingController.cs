using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using JourneySpire.Models;

namespace JourneySpire.Controllers
{
    public class BookingController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            List<Booking> list = new List<Booking>();
            try 
            { 
              using (SqlConnection con = GetConnection())
              {
                SqlCommand cmd = new SqlCommand("sp_Booking_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Booking b = new Booking(); 
                        b.BookingId = Convert.ToInt32(dr["BookingId"]); 
                        b.PackageName = dr["PackageName"].ToString();
                        b.CustomerName = dr["CustomerName"].ToString();
                        b.Email = dr["Email"].ToString();
                        b.Phone = dr["Phone"].ToString();
                        b.TravelDate = Convert.ToDateTime(dr["TravelDate"]);
                        b.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        b.NumberOfPersons = Convert.ToInt32(dr["NumberOfPersons"]);
                        b.Price = Convert.ToDecimal(dr["Price"]);
                    b.TotalAmount = dr["TotalAmount"] == DBNull.Value ? b.Price * b.NumberOfPersons :
                    Convert.ToDecimal(dr["TotalAmount"]);
                        b.BookingStatus = dr["BookingStatus"].ToString();
                   

                    list.Add(b);
                }
              }
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
            return View(list);
        }
            

        // ================= CREATE =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Booking model)
        {
            try
            {
                // AUTO CALCULATION
                model.TotalAmount = model.Price * model.NumberOfPersons;

                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Booking_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PackageId", model.PackageId);
                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Phone", model.Phone);
                    cmd.Parameters.AddWithValue("@TravelDate", model.TravelDate);
                    cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                    cmd.Parameters.AddWithValue("@NumberOfPersons", model.NumberOfPersons);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
                    cmd.Parameters.AddWithValue("@BookingStatus", "Confirmed");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Create","Guide");
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ================= DETAILS =================
        public IActionResult Details(int id)
        {
            Booking b = new Booking();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Booking_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    b.BookingId = Convert.ToInt32(dr["BookingId"]);
                    b.PackageId = Convert.ToInt32(dr["PackageId"]);
                    b.PackageName = dr["PackageName"].ToString();
                    b.CustomerName = dr["CustomerName"].ToString();
                    b.Email = dr["Email"].ToString();
                    b.Phone = dr["Phone"].ToString();
                    b.TravelDate = Convert.ToDateTime(dr["TravelDate"]);
                    b.EndDate = Convert.ToDateTime(dr["EndDate"]);
                    b.NumberOfPersons = Convert.ToInt32(dr["NumberOfPersons"]);
                    b.Price = Convert.ToDecimal(dr["Price"]);
                    b.TotalAmount = dr["TotalAmount"] == DBNull.Value ? b.Price * b.NumberOfPersons :
                Convert.ToDecimal(dr["TotalAmount"]);
                    b.BookingStatus = dr["BookingStatus"].ToString();
                }
            }

            return View(b);
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            return Details(id);
        }

        [HttpPost]
        public IActionResult Edit(Booking model)
        {
            try
            {
                model.TotalAmount = model.Price * model.NumberOfPersons;

                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_Booking_Update", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PackageId", model.PackageId);
                    cmd.Parameters.AddWithValue("@BookingId", model.BookingId);
                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Phone", model.Phone);
                    cmd.Parameters.AddWithValue("@TravelDate", model.TravelDate);
                    cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                    cmd.Parameters.AddWithValue("@NumberOfPersons", model.NumberOfPersons);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);
                    cmd.Parameters.AddWithValue("@BookingStatus", model.BookingStatus);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ================= DELETE =================
        // GET: Booking/Delete/5  → Confirmation page
        public IActionResult Delete(int id)
        {
            Booking b = new Booking();

            using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                SqlCommand cmd = new SqlCommand("sp_Booking_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookingId", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    b.BookingId = Convert.ToInt32(dr["BookingId"]);
                    b.PackageName = dr["PackageName"].ToString();
                    b.CustomerName = dr["CustomerName"].ToString();
                    b.TotalAmount = dr["TotalAmount"] == DBNull.Value
                        ? 0
                        : Convert.ToDecimal(dr["TotalAmount"]);
                }
            }

            return View(b);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int BookingId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand("sp_Booking_Delete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BookingId", BookingId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                TempData["Success"] = "Booking deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

    }
}
