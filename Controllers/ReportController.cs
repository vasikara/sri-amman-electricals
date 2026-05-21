using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using JourneySpire.Models;
using ClosedXML.Excel;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System.IO;
using iText.Layout.Properties;

namespace JourneySpire.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _conn;

        public ReportController(IConfiguration configuration)
        {
            _conn = configuration.GetConnectionString("DefaultConnection");
        }

        // ================= INDEX =================
        public IActionResult Index()
        {
            List<Report> list = new List<Report>();

            try
            {
                using SqlConnection con = new SqlConnection(_conn);
                using SqlCommand cmd = new SqlCommand("sp_Report_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Report
                    {
                        ReportId = Convert.ToInt32(dr["ReportId"]),
                        ReportType = dr["ReportType"].ToString(),
                        FromDate = dr["FromDate"] as DateTime?,
                        ToDate = dr["ToDate"] as DateTime?,
                        GeneratedBy = dr["GeneratedBy"].ToString(),
                        GeneratedOn = Convert.ToDateTime(dr["GeneratedOn"])
                    });
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View(list);
        }

        // ================= CREATE =================
        public IActionResult Create() => View();

        [HttpPost]
        public IActionResult Create(Report model)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_conn);
                using SqlCommand cmd = new SqlCommand("sp_Report_Insert", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReportType", model.ReportType);
                cmd.Parameters.AddWithValue("@FromDate", (object?)model.FromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object?)model.ToDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GeneratedBy", "Admin");

                con.Open();
                cmd.ExecuteNonQuery();

                TempData["Success"] = "Report created successfully";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        public IActionResult Edit(int id)
        {
            return View(GetReportById(id));
        }

        [HttpPost]
        public IActionResult Edit(Report model)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_conn);
                using SqlCommand cmd = new SqlCommand("sp_Report_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReportId", model.ReportId);
                cmd.Parameters.AddWithValue("@FromDate", (object?)model.FromDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDate", (object?)model.ToDate ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        public IActionResult Delete(int id)
        {
            return View(GetReportById(id));
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int reportId)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_conn);
                using SqlCommand cmd = new SqlCommand("sp_Report_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ReportId", reportId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        // ================= DETAILS =================
        public IActionResult Details(int id)
        {
            try
            {
                Report report = GetReportById(id);
                ViewBag.ReportType = report.ReportType;
                ViewBag.ReportId = id;

                if (report.ReportType == "Booking")
                    ViewBag.BookingReport = GetBookingReport(id);
                else if (report.ReportType == "Payment")
                    ViewBag.PaymentReport = GetPaymentReport(id);
                else if (report.ReportType == "Guide")
                    ViewBag.GuideReport = GetGuideReport(id);

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        // ================= EXCEL =================
        public IActionResult ExportToExcel(int reportId, string reportType)
        {
            using XLWorkbook wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Report");
            int row = 1;

            if (reportType == "Booking")
            {
                var list = GetBookingReport(reportId);
                ws.Cell(row, 1).Value = "BookingId";
                ws.Cell(row, 2).Value = "Customer";
                ws.Cell(row, 3).Value = "Package";
                ws.Cell(row, 4).Value = "Amount";
                row++;

                foreach (var i in list)
                {
                    ws.Cell(row, 1).Value = i.BookingId;
                    ws.Cell(row, 2).Value = i.CustomerName;
                    ws.Cell(row, 3).Value = i.PackageName;
                    ws.Cell(row, 4).Value = i.TotalAmount;
                    row++;
                }
            }

            using MemoryStream ms = new MemoryStream();
            wb.SaveAs(ms);

            return File(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{reportType}_Report.xlsx");
        }

        // ================= PDF =================
        public IActionResult ExportToPdf(int reportId, string reportType)
        {
            using MemoryStream ms = new MemoryStream();
            PdfWriter writer = new PdfWriter(ms);
            PdfDocument pdf = new PdfDocument(writer);
            Document doc = new Document(pdf);

            doc.Add(new Paragraph($"{reportType} Report").SetFontSize(16).SetTextAlignment(TextAlignment.CENTER));

            if (reportType == "Booking")
            {
                var list = GetBookingReport(reportId);
                Table table = new Table(4);
                table.AddHeaderCell("BookingId");
                table.AddHeaderCell("Customer");
                table.AddHeaderCell("Package");
                table.AddHeaderCell("Amount");

                foreach (var i in list)
                {
                    table.AddCell(i.BookingId.ToString());
                    table.AddCell(i.CustomerName);
                    table.AddCell(i.PackageName);
                    table.AddCell(i.TotalAmount.ToString());
                }
                doc.Add(table);
            }

            doc.Close();
            return File(ms.ToArray(), "application/pdf", $"{reportType}_Report.pdf");
        }

        // ================= HELPERS =================
        private Report GetReportById(int id)
        {
            Report r = new Report();

            using SqlConnection con = new SqlConnection(_conn);
            using SqlCommand cmd = new SqlCommand("sp_Report_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportId", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                r.ReportId = id;
                r.ReportType = dr["ReportType"].ToString();
                r.FromDate = dr["FromDate"] as DateTime?;
                r.ToDate = dr["ToDate"] as DateTime?;
                r.GeneratedBy = dr["GeneratedBy"].ToString();
                r.GeneratedOn = Convert.ToDateTime(dr["GeneratedOn"]);
            }
            return r;
        }

        private List<BookingReport> GetBookingReport(int reportId)
        {
            List<BookingReport> list = new List<BookingReport>();

            using SqlConnection con = new SqlConnection(_conn);
            using SqlCommand cmd = new SqlCommand("sp_Booking_Report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportId", reportId);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new BookingReport
                {
                    BookingId = Convert.ToInt32(dr["BookingId"]),
                    CustomerName = dr["CustomerName"].ToString(),
                    PackageName = dr["PackageName"].ToString(),
                    TotalAmount = Convert.ToDecimal(dr["TotalAmount"])
                });
            }
            return list;
        }

        private List<PaymentReport> GetPaymentReport(int reportId)
        {
            return new List<PaymentReport>(); // same pattern
        }

        private List<GuideReport> GetGuideReport(int reportId)
        {
            return new List<GuideReport>(); // same pattern
        }
    }
}
