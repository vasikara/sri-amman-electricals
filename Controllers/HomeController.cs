using System.Data.SqlClient;
using System.Diagnostics;
using JourneySpire.Models;
using Microsoft.AspNetCore.Mvc;

namespace JourneySpire.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration; 
        private readonly IWebHostEnvironment _env;

        public HomeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private string GetConnection()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<TourPackage> list = new List<TourPackage>();

            try
            {
                using (SqlConnection con = new SqlConnection( _configuration.GetConnectionString("DefaultConnection")))
                {
                    SqlCommand cmd = new SqlCommand("SELECT PackageId,PacageName,Location,Days, Price,Description,ImagePath FROM TourPackages WHERE IsActive = 1", con);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        TourPackage p = new TourPackage();
                        {
                            p.PackageId = Convert.ToInt32(dr["PackageId"]);
                            p.PackageName = dr["PackageName"].ToString();
                            p.Location = dr["Location"].ToString();
                            p.Days = Convert.ToInt32(dr["Days"]);
                            p.Price = Convert.ToDecimal(dr["Price"]);
                            p.Description = dr["Description"].ToString();
                            p.ImagePath = dr["ImagePath"].ToString();
                        }

                    }
                    dr.Close();
                    con.Close();
                } 
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }
 
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
