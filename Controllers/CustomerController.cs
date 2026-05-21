using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

public class CustomerController : Controller
{
    private readonly IConfiguration _configuration;

    public CustomerController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // SQL Connection
    private SqlConnection GetConnection()
    {
        return new SqlConnection(
            _configuration.GetConnectionString("DefaultConnection"));
    }


    // GET
    public IActionResult Login()
    {
        return View();
    }

    // POST
    [HttpPost]
    public IActionResult Login(string Email, string Password)
    {
        // Validate email & password from DB
        // If success → RedirectToAction("Index", "Home")
        // Else → ViewBag.Error = "Invalid email or password"
        try
        {
            return RedirectToAction("Index","Home");
        }
        catch 
        { 
           return View();
        }
    }


    // =========================
    // INDEX – GET ALL CUSTOMERS
    // =========================
    public IActionResult Index()
    {
        List<Customer> list = new List<Customer>();

        try
        {
            using SqlConnection con = GetConnection();
            SqlCommand cmd = new("sp_Customer_GetAll", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                list.Add(new Customer
                {
                    CustomerId = Convert.ToInt32(dr["CustomerId"]),
                    FullName = dr["FullName"].ToString(),
                    Email = dr["Email"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    Address = dr["Address"]?.ToString(),
                    CreatedDate = Convert.ToDateTime(dr["CreatedDate"])
                });
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
        }

        return View(list);
    }

    // =========================
    // CREATE – GET
    // =========================
    public IActionResult Create()
    {
        return View();
    }

    // =========================
    // CREATE – POST
    // =========================
    [HttpPost]
    public IActionResult Create(Customer model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            using SqlConnection con = GetConnection();
            SqlCommand cmd = new("sp_Customer_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FullName", model.FullName);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Phone", model.Phone);
            cmd.Parameters.AddWithValue("@Address", model.Address ?? "");

            con.Open();
            cmd.ExecuteNonQuery();

            TempData["Success"] = "Customer registered successfully";
            return RedirectToAction("Login");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    // =========================
    // DETAILS – GET
    // =========================
    public IActionResult Details(int id)
    {
        Customer customer = new();

        try
        {
            using SqlConnection con = GetConnection();
            SqlCommand cmd = new("sp_Customer_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerId", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                customer.CustomerId = Convert.ToInt32(dr["CustomerId"]);
                customer.FullName = dr["FullName"].ToString();
                customer.Email = dr["Email"].ToString();
                customer.Phone = dr["Phone"].ToString();
                customer.Address = dr["Address"]?.ToString();
                customer.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
        }

        return View(customer);
    }

    // =========================
    // EDIT – GET
    // =========================
    public IActionResult Edit(int id)
    {
        return Details(id);
    }

    // =========================
    // EDIT – POST
    // =========================
    [HttpPost]
    public IActionResult Edit(Customer model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            using SqlConnection con = GetConnection();
            SqlCommand cmd = new("sp_Customer_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId);
            cmd.Parameters.AddWithValue("@FullName", model.FullName);
            cmd.Parameters.AddWithValue("@Email", model.Email);
            cmd.Parameters.AddWithValue("@Phone", model.Phone);
            cmd.Parameters.AddWithValue("@Address", model.Address ?? "");

            con.Open();
            cmd.ExecuteNonQuery();

            TempData["Success"] = "Customer updated successfully";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.Error = ex.Message;
            return View(model);
        }
    }

    // =========================
    // DELETE – GET
    // =========================
    public IActionResult Delete(int id)
    {
        return Details(id);
    }

    // =========================
    // DELETE – POST
    // =========================
    [HttpPost]
    public IActionResult Delete(Customer model)
    {
        try
        {
            using SqlConnection con = GetConnection();
            SqlCommand cmd = new("sp_Customer_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId);

            con.Open();
            cmd.ExecuteNonQuery();

            TempData["Success"] = "Customer deleted successfully";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction("Index");
    }
}
