using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace JourneySpire.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IConfiguration _configuration;

        public ScheduleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Schedules ORDER BY ScheduleId DESC", con);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return View(dt);
        }

        // ================= CREATE =================
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Schedules
                    (VehicleId, DriverId, RouteId, ScheduleDate, StartTime, EndTime, Status)
                    VALUES
                    (@VehicleId, @DriverId, @RouteId, @ScheduleDate, @StartTime, @EndTime, @Status)", con);

                cmd.Parameters.AddWithValue("@VehicleId", form["VehicleId"]);
                cmd.Parameters.AddWithValue("@DriverId", form["DriverId"]);
                cmd.Parameters.AddWithValue("@RouteId", form["RouteId"]);
                cmd.Parameters.AddWithValue("@ScheduleDate", form["ScheduleDate"]);
                cmd.Parameters.AddWithValue("@StartTime", form["StartTime"]);
                cmd.Parameters.AddWithValue("@EndTime", form["EndTime"]);
                cmd.Parameters.AddWithValue("@Status", form["Status"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "SELECT * FROM Schedules WHERE ScheduleId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return View(dt.Rows[0]);
        }

        [HttpPost]
        public IActionResult Edit(IFormCollection form)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(@"
                    UPDATE Schedules SET
                        VehicleId=@VehicleId,
                        DriverId=@DriverId,
                        RouteId=@RouteId,
                        ScheduleDate=@ScheduleDate,
                        StartTime=@StartTime,
                        EndTime=@EndTime,
                        Status=@Status
                    WHERE ScheduleId=@ScheduleId", con);

                cmd.Parameters.AddWithValue("@ScheduleId", form["ScheduleId"]);
                cmd.Parameters.AddWithValue("@VehicleId", form["VehicleId"]);
                cmd.Parameters.AddWithValue("@DriverId", form["DriverId"]);
                cmd.Parameters.AddWithValue("@RouteId", form["RouteId"]);
                cmd.Parameters.AddWithValue("@ScheduleDate", form["ScheduleDate"]);
                cmd.Parameters.AddWithValue("@StartTime", form["StartTime"]);
                cmd.Parameters.AddWithValue("@EndTime", form["EndTime"]);
                cmd.Parameters.AddWithValue("@Status", form["Status"]);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // DELETE 
        public IActionResult Delete(int id)
        {
            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    "DELETE FROM Schedules WHERE ScheduleId=@id", con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
