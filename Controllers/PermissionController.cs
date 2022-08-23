using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

using Newtonsoft.Json;

namespace HR.Controllers
{
    public class PermissionController : Controller
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["SqlExpress"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        public string GetMemberPermissions()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetMemberPermissions", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberId", DBNull.Value);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }
    }
}