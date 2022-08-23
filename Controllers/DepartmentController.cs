using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using HR.Models;

namespace HR.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["SqlExpress"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Get()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return Json(dt.AsEnumerable().Select(r => new
            {
                Id = (int)r["Id"],
                Name = (string)r["Name"]
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Add(Department department)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("AddDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter idParam = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(idParam);
                cmd.Parameters.AddWithValue("@Name", department.Name);

                con.Open();
                cmd.ExecuteNonQuery();

                department.Id = (int)idParam.Value;
            }

            return Json(department);
        }

        [HttpPost]
        public JsonResult Update(Department department)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("UpdateDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", department.Id);
                cmd.Parameters.AddWithValue("@Name", department.Name);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(department);
        }

        [HttpPost]
        public JsonResult Delete(Department department)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("DeleteDepartment", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", department.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(department);
        }
    }
}
