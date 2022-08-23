using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;

using HR.Models;

using Newtonsoft.Json;

namespace HR.Controllers
{
    public class HomeController : Controller
    {
        private readonly string conStr = ConfigurationManager.ConnectionStrings["SqlExpress"].ConnectionString;

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetAllTeamMember()
        {
            DataTable dt = GetData("GetAllTeamMember");
            //return Json(TeamMember.FetchFromDataTable(dt), JsonRequestBehavior.AllowGet);
            return new JsonResult()
            {
                Data = TeamMember.FetchFromDataTable(dt),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public string GetEmploymentStatus()
        {
            DataTable dt = GetData("GetEmploymentStatus");
            return JsonConvert.SerializeObject(dt);
        }

        public string GetPermission()
        {
            DataTable dt = GetData("GetPermission");
            return JsonConvert.SerializeObject(dt);
        }

        public string GetShift()
        {
            DataTable dt = GetData("GetShift");
            return JsonConvert.SerializeObject(dt);
        }

        public string GetActivityLog(int MemberId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetActivityLog", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberId", MemberId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }

        public string GetMemberPermissions(int MemberId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("GetMemberPermissions", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MemberId", MemberId);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return JsonConvert.SerializeObject(dt);
        }

        [HttpPost]
        public JsonResult SetMemberPermission(int MemberId, int[] PermissionIds)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("SetMemberPermission", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TeamMemberId", MemberId);

                SqlParameter permissionId = new SqlParameter()
                {
                    ParameterName = "@PermissionId",
                    SqlDbType = SqlDbType.Int
                };

                cmd.Parameters.Add(permissionId);

                con.Open();

                foreach (int id in PermissionIds)
                {
                    permissionId.Value = id;
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(new { });
        }

        public string AddTeamMember(TeamMember Member)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("AddTeamMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter idParam = new SqlParameter()
                {
                    ParameterName = "@Id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };

                cmd.Parameters.Add(idParam);
                cmd.Parameters.AddWithValue("@Name", Member.Name);
                cmd.Parameters.AddWithValue("@Address", (object)Member.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailAddress", (object)Member.EmailAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)Member.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PositionId", (object)Member.Position?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DepartmentId", (object)Member.Department?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object)Member.StartDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmploymentStatusId", (object)Member.EmploymentStatus?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ShiftId", (object)Member.Shift?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ManagerId", (object)Member.Manager?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Photo", (object)Member.Photo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FavoriteColor", (object)Member.FavoriteColor ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();

                Member.Id = (int)idParam.Value;
            }

            return JsonConvert.SerializeObject(Member);
        }

        public string UpdateTeamMember(TeamMember Member)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("UpdateTeamMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Member.Id);
                cmd.Parameters.AddWithValue("@Name", Member.Name);
                cmd.Parameters.AddWithValue("@Address", (object)Member.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmailAddress", (object)Member.EmailAddress ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PhoneNumber", (object)Member.PhoneNumber ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PositionId", (object)Member.Position?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DepartmentId", (object)Member.Department?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@StartDate", (object)Member.StartDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EmploymentStatusId", (object)Member.EmploymentStatus?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ShiftId", (object)Member.Shift?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ManagerId", (object)Member.Manager?.Id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Photo", (object)Member.Photo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FavoriteColor", (object)Member.FavoriteColor ?? DBNull.Value);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            if (Member.Department?.Id != null && (Member.DepartmentName == null || Member.DepartmentName != Member.Department.Name))
            {
                Member.DepartmentName = Member.Department.Name;
            }

            if (Member.Manager?.Id != null && (Member.ManagerName == null || Member.ManagerName != Member.Manager.Name))
            {
                Member.ManagerName = Member.Manager.Name;
            }

            return JsonConvert.SerializeObject(Member);
        }

        public string DeleteTeamMember(TeamMember Member)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("DeleteTeamMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", Member.Id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return JsonConvert.SerializeObject(Member);
        }

        public ActionResult TerminateTeamMember(int MemberId)
        {
            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand("TerminateTeamMember", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", MemberId);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { });
        }

        [HttpPost]
        public JsonResult UploadFile()
        {
            try
            {
                HttpPostedFileBase file = Request.Files["photo"];
                int id = Convert.ToInt32(Request.Form["MemberId"]);

                if (file.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/UploadedFiles"), fileName);

                    Directory.CreateDirectory(Server.MapPath("~/UploadedFiles"));
                    file.SaveAs(filePath);

                    using (SqlConnection con = new SqlConnection(conStr))
                    using (SqlCommand cmd = new SqlCommand("UpdateMemberPhoto", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.Parameters.AddWithValue("@Photo", "/UploadedFiles/" + fileName);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }

                    return Json(new
                    {
                        FilePath = "/UploadedFiles/" + fileName
                    });
                }
                else
                {
                    return Json(new
                    {
                        FilePath = ""
                    });
                }
            }
            catch
            {
                return Json(new
                {
                    FilePath = ""
                });
            }
        }

        private DataTable GetData(string StoredName)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(conStr))
            using (SqlCommand cmd = new SqlCommand(StoredName, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }
    }
}