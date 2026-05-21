using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using JourneySpire.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;
using System.IO;
using AspNetCoreGeneratedDocument;

namespace JourneySpire.Controllers
{
    public class TourPackageController : Controller
    {
        private readonly IConfiguration _configuration;

        public TourPackageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection"));
        }

        // INDEX
        public IActionResult Index()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_TourPackage_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return View(dt);
        }

        // CREATE GET
        public IActionResult Create()
        {
            return View();
        }

        // CREATE POST
        [HttpPost]
        public IActionResult Create(TourPackage model, IFormFile ImageFile)
        {
            try
            { 
                string imagePath = null;

                if (ImageFile != null)
                {
                    string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    Directory.CreateDirectory(folder);

                    string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                    string fullPath = Path.Combine(folder, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        ImageFile.CopyTo(stream);
                    }

                    imagePath = "/images/" + fileName;
                }

                using (SqlConnection con = GetConnection())
                {
                    SqlCommand cmd = new SqlCommand("sp_TourPackage_Insert", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                    cmd.Parameters.AddWithValue("@Location", model.Location);
                    cmd.Parameters.AddWithValue("@Days", model.Days);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.Parameters.AddWithValue("@ImagePath", imagePath);

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

        // EDIT GET
        public IActionResult Edit(int id)
        {
            TourPackage model = new TourPackage();

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_TourPackage_GetById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PackageId", id);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    model.PackageId = (int)rdr["PackageId"];
                    model.PackageName = rdr["PackageName"].ToString();
                    model.Location = rdr["Location"].ToString();
                    model.Days = (int)rdr["Days"];
                    model.Price = (decimal)rdr["Price"];
                    model.Description = rdr["Description"].ToString();
                    model.ImagePath = rdr["ImagePath"].ToString();
                }
            }

            return View(model);
        }

        // EDIT POST
        [HttpPost]
        public IActionResult Edit(TourPackage model, IFormFile ImageFile)
        {
            string imagePath = model.ImagePath;

            if (ImageFile != null)
            {
                string folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string fullPath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                imagePath = "/images/" + fileName;
            }

            using (SqlConnection con = GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_TourPackage_Update", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PackageId", model.PackageId);
                cmd.Parameters.AddWithValue("@PackageName", model.PackageName);
                cmd.Parameters.AddWithValue("@Location", model.Location);
                cmd.Parameters.AddWithValue("@Days", model.Days);
                cmd.Parameters.AddWithValue("@Price", model.Price);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@ImagePath", imagePath);

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
                SqlCommand cmd = new SqlCommand("sp_TourPackage_Delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PackageId", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
