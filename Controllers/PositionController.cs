using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

using HR.Models;

namespace HR.Controllers
{
    public class PositionController : Controller
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
            using (SqlCommand cmd = new SqlCommand("GetPosition", con))
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
        public JsonResult Add(Position position)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("AddPosition", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter idParam = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(idParam);
                cmd.Parameters.AddWithValue("@Name", position.Name);

                con.Open();
                cmd.ExecuteNonQuery();

                position.Id = (int)idParam.Value;
            }

            return Json(position);
        }

        [HttpPost]
        public JsonResult Update(Position position)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("UpdatePosition", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", position.Id);
                cmd.Parameters.AddWithValue("@Name", position.Name);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(position);
        }

        [HttpPost]
        public JsonResult Delete(Position position)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("DeletePosition", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", position.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(position);
        }
    }
}
