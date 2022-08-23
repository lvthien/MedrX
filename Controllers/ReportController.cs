using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace HR.Controllers
{
    public class ReportController : Controller
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["SqlExpress"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public string GetWeeklyHire()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetWeeklyHire", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }

        public string GetTerminatedCurrentYear()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetTerminatedCurrentYear", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }
    }
}